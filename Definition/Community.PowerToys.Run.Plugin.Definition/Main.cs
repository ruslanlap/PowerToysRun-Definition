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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
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
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
            return client;
        });
        private static HttpClient HttpClient => HttpClientLazy.Value;

        private readonly LRUCache _cache = new(ConfigurationManager.Configuration.CacheMaxSize);
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

            CancelPreviousRequest();

            // Handle empty query
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Result> { CreateInfoResult(rawSearch, Name, EmptyQueryMessage) };
            }

            // Parse language prefix (e.g., "fr:bonjour", "en:hello")
            string forcedLang = null;
            var termForLookup = searchTerm;
            var colonIndex = searchTerm.IndexOf(':');
            if (colonIndex > 0 && colonIndex < searchTerm.Length - 1)
            {
                var prefix = searchTerm.Substring(0, colonIndex);
                if (_dictionaryProviders.ContainsKey(prefix))
                {
                    forcedLang = prefix;
                    termForLookup = searchTerm.Substring(colonIndex + 1).Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(termForLookup))
            {
                return new List<Result> { CreateInfoResult(rawSearch, Name, EmptyQueryMessage) };
            }

            // Build cache key that includes the forced language
            var cacheKey = forcedLang != null ? $"{forcedLang}:{termForLookup}" : termForLookup;

            // Check cache first
            if (TryGetCachedResults(cacheKey, rawSearch, out var cachedResults))
            {
                return cachedResults;
            }

            // Show loading message for non-delayed execution
            if (!delayedExecution)
            {
                return new List<Result> { CreateInfoResult(rawSearch, SearchingMessage, $"Searching for '{termForLookup}'") };
            }

            // Perform actual API call
            return ExecuteDelayedQuery(termForLookup, rawSearch, cacheKey, forcedLang);
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

        private List<Result> ExecuteDelayedQuery(string searchTerm, string rawSearch, string cacheKey = null, string forcedLang = null)
        {
            try
            {
                // Use Task.Run to avoid blocking the UI thread
                var task = Task.Run(async () => await FetchAndProcessResultsAsync(searchTerm, rawSearch, _cancellationTokenSource.Token, forcedLang));
                var results = task.ConfigureAwait(false).GetAwaiter().GetResult();

                CacheResults(cacheKey ?? searchTerm, results);
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
                else if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) hasLatin = true;
            }

            if (hasCyrillic && !hasCjk && !hasLatin) return ScriptType.Cyrillic;
            if (hasCjk && !hasCyrillic && !hasLatin) return ScriptType.Cjk;
            if (hasLatin && !hasCyrillic && !hasCjk) return ScriptType.Latin;
            return ScriptType.Mixed;
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

        private async Task<List<Result>> FetchAndProcessResultsAsync(string searchTerm, string rawSearch, CancellationToken cancellationToken, string forcedLang = null)
        {
            return await RetryHelper.RetryAsync(async () =>
            {
                List<IDictionaryProvider> providers;
                ScriptType script;

                if (forcedLang != null && _dictionaryProviders.TryGetValue(forcedLang, out var forcedProvider))
                {
                    providers = new List<IDictionaryProvider> { forcedProvider };
                    script = DetectScript(searchTerm);
                }
                else
                {
                    script = DetectScript(searchTerm);
                    providers = GetProvidersForScript(script).ToList();
                }
                
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

                var results = ProcessDictionaryEntries(allEntries, rawSearch);
                
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

        private IDictionaryProvider GetCurrentProvider()
        {
            var lang = ConfigurationManager.Configuration.Language?.ToLowerInvariant() ?? "en";
            if (_dictionaryProviders.TryGetValue(lang, out var provider))
            {
                return provider;
            }

            return _dictionaryProviders["en"];
        }

        private List<Result> ProcessDictionaryEntries(List<DictionaryEntry> entries, string rawSearch)
        {
            var results = new List<Result>();
            var resultProcessor = new ResultProcessor(_iconManager);

            foreach (var entry in entries.Where(e => e != null))
            {
                var entryResults = resultProcessor.ProcessEntry(entry, rawSearch);
                results.AddRange(entryResults);
            }

            return results;
        }
        #endregion

        #region Context Menu
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            if (!(selectedResult.ContextData is ResultContext context))
            {
                return new List<ContextMenuResult>();
            }

            var menuBuilder = new ContextMenuBuilder(Name, _context, _audioManager);
            return menuBuilder.BuildMenuItems(context, selectedResult);
        }
        #endregion

        #region Result Creation Helpers
        private Result CreateInfoResult(string rawSearch, string title, string subTitle) => new()
        {
            QueryTextDisplay = rawSearch,
            IcoPath = _iconManager.InfoIcon,
            Title = title,
            SubTitle = subTitle,
            Action = _ => false,
            ContextData = null
        };

        private Result CreateErrorResult(string rawSearch, string title, string subTitle) => new()
        {
            QueryTextDisplay = rawSearch,
            IcoPath = _iconManager.ErrorIcon,
            Title = title,
            SubTitle = subTitle,
            Action = _ => false,
            ContextData = null
        };
        #endregion

        #region Disposal
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_context?.API != null)
                {
                    _context.API.ThemeChanged -= OnThemeChanged;
                }
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _audioManager?.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}