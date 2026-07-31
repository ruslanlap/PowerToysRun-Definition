using ManagedCommon;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Definition
{
    public class Main : IPlugin, IDelayedExecutionPlugin, IContextMenu, IDisposable
    {
        #region Constants
        public static string PluginID => "AF6979212B9D429489F115EE3390D608";
        public string Name => "Definition";
        public string Description => "Lookup word definitions, phonetics, synonyms, antonyms, and examples.";

        // UI strings
        private const string EmptyQueryMessage = "Type a word to look up...";
        private const string SearchingMessage = "Looking up...";
        private const string NetworkErrorTitle = "Network Error";
        private const string ApiErrorTitle = "API Error";
        private const string UnexpectedErrorTitle = "Error";
        #endregion

        #region Fields
        private string _pluginDirectory;
        private readonly IconManager _iconManager;
        private PluginInitContext _context;
        private bool _disposed;

        private static readonly Lazy<HttpClient> HttpClientLazy = new(() =>
        {
            var handler = new HttpClientHandler();
            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(ConfigurationManager.Configuration.HttpTimeoutSeconds)
            };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("PowerToysRun-Definition/1.5.3 (https://github.com/ruslanlap/PowerToysRun-Definition)");
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
            return client;
        });
        
        private static HttpClient HttpClient => HttpClientLazy.Value;

        private LRUCache _cache = new LRUCache(ConfigurationManager.Configuration.CacheMaxSize);
        private readonly AudioManager _audioManager;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Dictionary<string, IDictionaryProvider> _dictionaryProviders;
        #endregion

        #region Initialization
        public Main()
        {
            _iconManager = new IconManager();
            _audioManager = new AudioManager();
            _dictionaryProviders = new Dictionary<string, IDictionaryProvider>(StringComparer.OrdinalIgnoreCase)
            {
                { "en", new EnglishDictionaryProvider(HttpClient) },
                { "fr", new FrenchDictionaryProvider(HttpClient) },
                { "it", new ItalianDictionaryProvider(HttpClient) },
                { "uk", new UkrainianDictionaryProvider(HttpClient) },
                { "zh", new ChineseDictionaryProvider(HttpClient) }
            };
        }

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.API.ThemeChanged += OnThemeChanged;

            _pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _iconManager.Initialize(_pluginDirectory, _context.API.GetCurrentTheme());

            Debug.WriteLine($"[Definition Plugin] Initialized. Directory: {_pluginDirectory}");
        }
        #endregion

        #region Theme Management
        private void OnThemeChanged(Theme _, Theme newTheme) => _iconManager.UpdateTheme(newTheme);
        #endregion

        #region Query Processing
        public List<Result> Query(Query query) => Query(query, false);

        public List<Result> Query(Query query, bool delayedExecution)
        {
            // Reload configuration to pick up changes
            ConfigurationManager.ReloadConfiguration();
            
            var rawSearch = query.Search ?? string.Empty;
            var searchTerm = rawSearch.Trim().ToLowerInvariant();
            
            // Parse subcommand (pronunciation, synonyms, antonyms, examples)
            var (subcommand, searchWord) = ParseSubcommand(searchTerm);
            if (string.IsNullOrWhiteSpace(searchWord))
            {
                return new List<Result> { CreateInfoResult(rawSearch, Name, EmptyQueryMessage) };
            }
            
            CancelPreviousRequest();

            // Handle empty query after subcommand parsing
            if (string.IsNullOrWhiteSpace(searchWord))
            {
                return new List<Result> { CreateInfoResult(rawSearch, Name, EmptyQueryMessage) };
            }

            // Check cache first
            if (TryGetCachedResults(searchTerm, rawSearch, out var cachedResults))
            {
                return cachedResults;
            }

            // Show loading message for non-delayed execution
            if (!delayedExecution)
            {
                return new List<Result> { CreateInfoResult(rawSearch, SearchingMessage, $"Searching for '{searchWord}'") };
            }

            // Perform actual API call
            return ExecuteDelayedQuery(searchWord, rawSearch, subcommand, searchTerm);
        }

        private static (string Subcommand, string SearchWord) ParseSubcommand(string input)
        {
            var parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) return (string.Empty, input.Trim());
            
            var potentialSubcommand = parts[0].ToLowerInvariant();
            var validSubcommands = new HashSet<string> { "pronunciation", "pron", "synonyms", "syn", "antonyms", "ant", "examples", "ex" };
            
            return validSubcommands.Contains(potentialSubcommand)
                ? (potentialSubcommand, parts[1].Trim())
                : (string.Empty, input.Trim());
        }

        private void CancelPreviousRequest()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private bool TryGetCachedResults(string searchTerm, string rawSearch, out List<Result> results)
        {
            if (_cache.TryGetValue(searchTerm, out var cacheItem))
            {
                results = cacheItem.Results.Select(r => r.Clone(rawSearch)).ToList();
                return true;
            }

            results = null;
            return false;
        }

        private List<Result> ExecuteDelayedQuery(string searchTerm, string rawSearch, string subcommand, string cacheKey)
        {
            try
            {
                // Use Task.Run to avoid blocking the UI thread
                var task = Task.Run(async () => await FetchAndProcessResultsAsync(searchTerm, rawSearch, subcommand, _cancellationTokenSource.Token));
                var results = task.ConfigureAwait(false).GetAwaiter().GetResult();
                
                CacheResults(cacheKey, results);
                return results;
            }
            catch (OperationCanceledException)
            {
                return new List<Result> { CreateInfoResult(rawSearch, "Searching...", "...") };
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"[Definition Plugin] HTTP error for '{searchTerm}': {ex.Message}");
                var errorMessage = ex.Data.Contains("StatusCode") 
                    ? $"Could not reach dictionary service. ({ex.Data["StatusCode"]})"
                    : "Could not reach dictionary service. Check connection.";
                return new List<Result> { CreateErrorResult(rawSearch, NetworkErrorTitle, errorMessage) };
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"[Definition Plugin] JSON parsing failed for '{searchTerm}': {ex.Message}");
                return new List<Result> { CreateErrorResult(rawSearch, ApiErrorTitle, "Failed to parse response from dictionary service.") };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Definition Plugin] Unexpected error for '{searchTerm}': {ex}");
                return new List<Result> { CreateErrorResult(rawSearch, UnexpectedErrorTitle, "An unexpected error occurred.") };
            }
        }

        private void CacheResults(string searchTerm, List<Result> results)
        {
            if (results.Any() && results.First().ContextData is ResultContext)
            {
                var cacheItem = new CacheItem(results, DateTime.UtcNow, searchTerm);
                _cache.Set(searchTerm, cacheItem);
            }
        }
        #endregion

        #region API Communication
        private static ScriptType DetectScript(string text)
        {
            bool hasCyrillic = false, hasCjk = false, hasLatin = false;
            foreach (var c in text)
            {
                if (c >= 0x0400 && c <= 0x04FF) hasCyrillic = true;
                else if ((c >= 0x4E00 && c <= 0x9FFF) || (c >= 0x3400 && c <= 0x4DBF)) hasCjk = true;
                else if (IsLatinCharacter(c)) hasLatin = true;
            }

            if (hasCyrillic && !hasCjk && !hasLatin) return ScriptType.Cyrillic;
            if (hasCjk && !hasCyrillic && !hasLatin) return ScriptType.Cjk;
            if (hasLatin && !hasCyrillic && !hasCjk) return ScriptType.Latin;
            return ScriptType.Mixed;
        }

        private static bool IsLatinCharacter(char c)
        {
            return (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z')
                || (c >= 0x00C0 && c <= 0x024F)
                || (c >= 0x1E00 && c <= 0x1EFF);
        }

        private IEnumerable<IDictionaryProvider> GetProvidersForScript(ScriptType script)
        {
            return script switch
            {
                ScriptType.Cyrillic => _dictionaryProviders.Values.Where(p => p.LanguageCode == "uk"),
                ScriptType.Cjk => _dictionaryProviders.Values.Where(p => p.LanguageCode == "zh"),
                ScriptType.Latin => GetLatinProviders(),
                _ => _dictionaryProviders.Values
            };
        }

        private IEnumerable<IDictionaryProvider> GetLatinProviders()
        {
            var latinLangs = (ConfigurationManager.Configuration.LatinLanguages ?? "en")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var providers = _dictionaryProviders.Values
                .Where(p => latinLangs.Any(l => string.Equals(l, p.LanguageCode, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return providers.Count > 0 ? providers : _dictionaryProviders.Values.Where(p => p.LanguageCode == "en");
        }

        private enum ScriptType { Latin, Cyrillic, Cjk, Mixed }

        private async Task<List<Result>> FetchAndProcessResultsAsync(string searchTerm, string rawSearch, string subcommand, CancellationToken cancellationToken)
        {
            return await RetryHelper.RetryAsync(async () =>
            {
                var script = DetectScript(searchTerm);
                var providers = GetProvidersForScript(script).ToList();

                Debug.WriteLine($"[Definition Plugin] Script: {script}, using providers: {string.Join(", ", providers.Select(p => p.LanguageCode))}");

                var tasks = providers.Select(async provider =>
                {
                    try
                    {
                        return await provider.LookupAsync(searchTerm, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[Definition Plugin] Provider {provider.LanguageCode} FAILED for '{searchTerm}': {ex.GetType().Name}: {ex.Message}");
                        return new List<DictionaryEntry>();
                    }
                }).ToList();
                
                var resultsList = await Task.WhenAll(tasks);
                var allEntries = resultsList.SelectMany(e => e ?? Enumerable.Empty<DictionaryEntry>()).ToList();

                if (!allEntries.Any())
                {
                    return new List<Result> { CreateInfoResult(rawSearch, $"No definitions found for '{searchTerm}'", "Check spelling or try another word.") };
                }

                var results = ProcessDictionaryEntries(allEntries, rawSearch, subcommand);
                
                // Filter results based on subcommand
                results = FilterResultsBySubcommand(results, subcommand);
                if (!results.Any())
                {
                    return new List<Result> { CreateInfoResult(rawSearch, $"No {GetSubcommandDisplayName(subcommand)} found for '{searchTerm}'", "Try another word or use the default definition lookup.") };
                }

                foreach (var result in results)
                {
                    if (result.ContextData is ResultContext)
                    {
                        bool isUkResult = result.SubTitle != null && result.SubTitle.Any(c => c >= 0x0400 && c <= 0x04FF);
                        bool isChineseResult = result.SubTitle != null && result.SubTitle.Any(c => (c >= 0x4E00 && c <= 0x9FFF) || (c >= 0x3400 && c <= 0x4DBF));

                        if (script == ScriptType.Cyrillic && isUkResult) result.Score += 10;
                        if (script == ScriptType.Cjk && isChineseResult) result.Score += 10;
                        if (script == ScriptType.Latin && !isUkResult && !isChineseResult) result.Score += 10;
                    }
                }

                return results.OrderByDescending(r => r.Score).ToList();
            }, cancellationToken, 3, $"Dictionary lookup for '{searchTerm}'");
        }

        private static string GetSubcommandDisplayName(string subcommand)
        {
            return subcommand switch
            {
                "pronunciation" or "pron" => "pronunciation",
                "synonyms" or "syn" => "synonyms",
                "antonyms" or "ant" => "antonyms",
                "examples" or "ex" => "examples",
                _ => "matching results"
            };
        }

        private static List<Result> FilterResultsBySubcommand(List<Result> results, string subcommand)
        {
            if (string.IsNullOrEmpty(subcommand))
                return results; // No subcommand - return all

            return subcommand switch
            {
                "pronunciation" or "pron" => results.Where(r =>
                    r.Title.StartsWith("Pronunciation:", StringComparison.OrdinalIgnoreCase)).ToList(),
                "synonyms" or "syn" => results.Where(r =>
                    r.Title.StartsWith("Synonyms (", StringComparison.OrdinalIgnoreCase)).ToList(),
                "antonyms" or "ant" => results.Where(r =>
                    r.Title.StartsWith("Antonyms (", StringComparison.OrdinalIgnoreCase)).ToList(),
                "examples" or "ex" => results.Where(r =>
                    r.Title.StartsWith("Example (", StringComparison.OrdinalIgnoreCase)).ToList(),
                _ => results
            };
        }

        private IDictionaryProvider GetCurrentProvider()
        {
            var lang = ConfigurationManager.Configuration.Language?.ToLowerInvariant() ?? "en";
            if (_dictionaryProviders.TryGetValue(lang, out var provider))
            {
                return provider;
            }
            
            return _dictionaryProviders["en"];
        }

        private List<Result> ProcessDictionaryEntries(List<DictionaryEntry> entries, string rawSearch, string subcommand)
        {
            var results = new List<Result>();
            var resultProcessor = new ResultProcessor(_iconManager);

            foreach (var entry in entries.Where(e => e != null))
            {
                var entryResults = resultProcessor.ProcessEntry(entry, rawSearch, subcommand);
                results.AddRange(entryResults);
            }

            return results.Any() 
                ? results 
                : new List<Result> { CreateResult(rawSearch, _iconManager.InfoIcon, "No definitions found", "No processable definitions in API response.", null, 0) };
        }
        #endregion

        #region Result Helpers
        private Result CreateInfoResult(string rawSearch, string title, string subtitle)
        {
            return CreateResult(rawSearch, _iconManager.InfoIcon, title, subtitle, null, 0);
        }

        private Result CreateErrorResult(string rawSearch, string title, string message)
        {
            return CreateResult(rawSearch, _iconManager.ErrorIcon, title, message, null, 0);
        }

        private Result CreateResult(string rawSearch, string iconPath, string title, string subtitle, ResultContext contextData, int score)
        {
            return new Result
            {
                QueryTextDisplay = rawSearch,
                IcoPath = iconPath,
                Title = title,
                SubTitle = subtitle,
                ContextData = contextData,
                Score = score,
                Action = _ => false
            };
        }
        #endregion

        #region Context Menu
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            if (selectedResult.ContextData is ResultContext context)
            {
                var contextMenuBuilder = new ContextMenuBuilder(Name, _context, _audioManager);
                return contextMenuBuilder.BuildMenuItems(context, selectedResult);
            }
            return new List<ContextMenuResult>();
        }
        #endregion

        #region Settings
        public Control CreateSettingPanel()
        {
            var panel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            panel.Children.Add(new TextBlock
            {
                Text = "No custom settings UI. Please use the built-in Additional Options in PowerToys."
            });

            var contentControl = new ContentControl
            {
                Content = panel
            };

            return contentControl;
        }

        public IEnumerable<PluginAdditionalOption> AdditionalOptions
        {
            get
            {
                var options = new List<PluginAdditionalOption>
                {
                    new PluginAdditionalOption
                    {
                        Key = nameof(PluginConfiguration.CacheMaxSize),
                        DisplayLabel = "Cache Size",
                        DisplayDescription = "Maximum number of cached dictionary entries",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                        TextValue = ConfigurationManager.Configuration.CacheMaxSize.ToString()
                    },
                    new PluginAdditionalOption
                    {
                        Key = nameof(PluginConfiguration.HttpTimeoutSeconds),
                        DisplayLabel = "HTTP Timeout (seconds)",
                        DisplayDescription = "Timeout for dictionary API requests",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                        TextValue = ConfigurationManager.Configuration.HttpTimeoutSeconds.ToString()
                    },
                    new PluginAdditionalOption
                    {
                        Key = nameof(PluginConfiguration.EnableAudioPlayback),
                        DisplayLabel = "Enable Audio Playback",
                        DisplayDescription = "Play pronunciation audio when available",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                        Value = ConfigurationManager.Configuration.EnableAudioPlayback
                    },
                    new PluginAdditionalOption
                    {
                        Key = nameof(PluginConfiguration.EnableClipboardOperations),
                        DisplayLabel = "Enable Clipboard Operations",
                        DisplayDescription = "Allow copying definitions to clipboard",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                        Value = ConfigurationManager.Configuration.EnableClipboardOperations
                    },
                    new PluginAdditionalOption
                    {
                        Key = nameof(PluginConfiguration.ShowExamplesInResults),
                        DisplayLabel = "Show Examples",
                        DisplayDescription = "Display usage examples in results",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                        Value = ConfigurationManager.Configuration.ShowExamplesInResults
                    },
                    new PluginAdditionalOption
                    {
                        Key = nameof(PluginConfiguration.ShowSynonymsInResults),
                        DisplayLabel = "Show Synonyms",
                        DisplayDescription = "Display synonyms in results",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                        Value = ConfigurationManager.Configuration.ShowSynonymsInResults
                    },
                    new PluginAdditionalOption
                    {
                        Key = nameof(PluginConfiguration.ShowAntonymsInResults),
                        DisplayLabel = "Show Antonyms",
                        DisplayDescription = "Display antonyms in results",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                        Value = ConfigurationManager.Configuration.ShowAntonymsInResults
                    }
                };

                return options;
            }
        }

        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            ConfigurationManager.UpdateConfiguration(config =>
            {
                if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PluginConfiguration.CacheMaxSize)) is var cacheOption && cacheOption != null)
                {
                    if (int.TryParse(cacheOption.TextValue, out var cacheMaxSize))
                        config.CacheMaxSize = cacheMaxSize;
                }
                
                if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PluginConfiguration.HttpTimeoutSeconds)) is var timeoutOption && timeoutOption != null)
                {
                    if (int.TryParse(timeoutOption.TextValue, out var httpTimeoutSeconds))
                        config.HttpTimeoutSeconds = httpTimeoutSeconds;
                }
                
                if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PluginConfiguration.EnableAudioPlayback)) is var audioOption && audioOption != null)
                    config.EnableAudioPlayback = audioOption.Value;
                
                if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PluginConfiguration.EnableClipboardOperations)) is var clipboardOption && clipboardOption != null)
                    config.EnableClipboardOperations = clipboardOption.Value;
                
                if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PluginConfiguration.ShowExamplesInResults)) is var examplesOption && examplesOption != null)
                    config.ShowExamplesInResults = examplesOption.Value;
                
                if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PluginConfiguration.ShowSynonymsInResults)) is var synonymsOption && synonymsOption != null)
                    config.ShowSynonymsInResults = synonymsOption.Value;
                
                if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PluginConfiguration.ShowAntonymsInResults)) is var antonymsOption && antonymsOption != null)
                    config.ShowAntonymsInResults = antonymsOption.Value;
            });
        }
        #endregion

        #region Cleanup
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            if (_context?.API != null)
            {
                _context.API.ThemeChanged -= OnThemeChanged;
            }

                        // LRUCache doesn't have Clear(), recreate it
            _cache = new LRUCache(ConfigurationManager.Configuration.CacheMaxSize);
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _audioManager?.Dispose();
            HttpClient?.Dispose();

            _disposed = true;
        }
        #endregion
    }
}