using ManagedCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media; 
using Wox.Plugin; 

namespace Community.PowerToys.Run.Plugin.Definition
{
    public class Main : IPlugin, IDelayedExecutionPlugin, IContextMenu, IDisposable
    {
        // --- Constants ---
        public static string PluginID => "AF6979212B9D429489F115EE3390D608";
        public string Name => "Definition";
        public string Description => "Lookup word definitions, phonetics, synonyms, antonyms, and examples.";
        private const string ApiEndpoint = "https://api.dictionaryapi.dev/api/v2/entries/en/";
        private const int CacheMaxSize = 100; 

        // --- Icons ---
        private string _pluginDirectory; 
        private string _iconDefinitionPath;
        private string _iconExamplePath;
        private string _iconSynonymPath;
        private string _iconAntonymPath;
        private string _iconErrorPath;
        private string _iconInfoPath;

        // --- State ---
        private PluginInitContext _context;
        private bool _disposed;
        private static HttpClient _httpClient; 
        private readonly Dictionary<string, List<Result>> _cache = new();
        private readonly MediaPlayer _mediaPlayer = new();
        private CancellationTokenSource _cancellationTokenSource; 

        // --- Initialization & Disposal ---
        public Main()
        {
            // Initialize static HttpClient only once
            if (_httpClient == null)
            {
                _httpClient = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(10)
                };
            }
        }

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.API.ThemeChanged += OnThemeChanged;

            // Get plugin directory from assembly location
            _pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            UpdateIconPaths(_context.API.GetCurrentTheme());

            // Log initialization info using Debug.WriteLine
            Debug.WriteLine($"[Definition Plugin] Plugin initialized. Directory: {_pluginDirectory}");
        }

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
                _mediaPlayer.Close(); 
                // Do not dispose static HttpClient: _httpClient?.Dispose();
            }

            _disposed = true;
        }

        // --- Theming ---
        private void UpdateIconPaths(Theme theme)
        {
            // Determine icon paths based on theme
            bool isLightTheme = theme == Theme.Light || theme == Theme.HighContrastWhite;

            // Use Path.Combine to create proper paths with the plugin directory
            _iconDefinitionPath = Path.Combine(_pluginDirectory, "Images", 
                isLightTheme ? "definition.light.png" : "definition.dark.png");

            _iconExamplePath = Path.Combine(_pluginDirectory, "Images", 
                isLightTheme ? "example.light.png" : "example.dark.png");

            _iconSynonymPath = Path.Combine(_pluginDirectory, "Images", 
                isLightTheme ? "synonym.light.png" : "synonym.dark.png");

            _iconAntonymPath = Path.Combine(_pluginDirectory, "Images", 
                isLightTheme ? "antonym.light.png" : "antonym.dark.png");

            _iconErrorPath = Path.Combine(_pluginDirectory, "Images", 
                isLightTheme ? "error.light.png" : "error.dark.png");

            _iconInfoPath = Path.Combine(_pluginDirectory, "Images", 
                isLightTheme ? "info.light.png" : "info.dark.png");

            // Log the paths for debugging
            Debug.WriteLine($"[Definition Plugin] Icon paths updated for theme: {theme}");
            Debug.WriteLine($"[Definition Plugin] Definition icon path: {_iconDefinitionPath}");

            // Verify icons exist
            if (File.Exists(_iconDefinitionPath))
            {
                Debug.WriteLine($"[Definition Plugin] Definition icon exists");
            }
            else
            {
                Debug.WriteLine($"[Definition Plugin] Definition icon not found at: {_iconDefinitionPath}");
            }
        }

        private void OnThemeChanged(Theme _, Theme newTheme) => UpdateIconPaths(newTheme);

        // --- Querying (Sync Wrapper) ---
        public List<Result> Query(Query query) => Query(query, false); 

        public List<Result> Query(Query query, bool delayedExecution) 
        {
            var rawSearch = query.Search ?? string.Empty;
            var searchTerm = rawSearch.Trim().ToLowerInvariant(); 

            // Cancel previous request if a new query comes in
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            // 1. Handle Empty Query
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Result> { CreateInfoResult(rawSearch, "Dictionary", "Type a word to look up...") };
            }

            // 2. Check Cache
            if (_cache.TryGetValue(searchTerm, out var cachedResults))
            {
                // Return cached results, updating QueryTextDisplay
                cachedResults.ForEach(r => r.QueryTextDisplay = rawSearch);
                return cachedResults;
            }

            // 3. Handle Non-Delayed Execution (Show Loading)
            if (!delayedExecution)
            {
                return new List<Result> { CreateInfoResult(rawSearch, "Looking up...", $"Searching for '{searchTerm}'") };
            }

            // 4. Perform Delayed Execution (Actual API Call)
            try
            {
                // Run the async fetch method and wait for its result synchronously
                // This is necessary because the Wox interface is synchronous.
                var results = FetchAndProcessResultsAsync(searchTerm, rawSearch, cancellationToken)
                                .GetAwaiter().GetResult();

                // Cache the results
                // Check if results list is not empty and the first result doesn't indicate an error or info message
                if (results.Any() && results.First().ContextData is ResultContext)
                {
                     if (_cache.Count >= CacheMaxSize) _cache.Clear(); 
                    _cache[searchTerm] = results;
                }

                return results;
            }
            catch (OperationCanceledException)
            {
                // Query was cancelled (user typed something else) - return empty list or loading indicator
                // No logging needed here.
                return new List<Result> { CreateInfoResult(rawSearch, "Searching...", "...") };
            }
            catch (HttpRequestException ex)
            {
                // Network or HTTP-related error
                Debug.WriteLine($"[Definition Plugin] HTTP request failed for '{searchTerm}': {ex.Message}");
                return new List<Result> { CreateErrorResult(rawSearch, "Network Error", $"Could not reach dictionary service. Check connection. ({ex.StatusCode})") };
            }
            catch (JsonException ex)
            {
                // Error parsing the JSON response
                Debug.WriteLine($"[Definition Plugin] JSON parsing failed for '{searchTerm}': {ex.Message}");
                return new List<Result> { CreateErrorResult(rawSearch, "API Error", "Failed to parse the response from the dictionary service.") };
            }
            catch (Exception ex)
            {
                // Catch-all for other unexpected errors
                Debug.WriteLine($"[Definition Plugin] Unexpected error during query for '{searchTerm}': {ex.ToString()}");
                return new List<Result> { CreateErrorResult(rawSearch, "Error", "An unexpected error occurred.") };
            }
        }

        // --- Asynchronous Data Fetching and Processing ---
        private async Task<List<Result>> FetchAndProcessResultsAsync(string searchTerm, string rawSearch, CancellationToken cancellationToken)
        {
            var requestUrl = $"{ApiEndpoint}{Uri.EscapeDataString(searchTerm)}";
            var results = new List<Result>();

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl, cancellationToken);

            // Handle "Not Found" specifically - this is not an error, just no results
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Return an info result, not an error result
                return new List<Result> { CreateInfoResult(rawSearch, $"No definitions found for '{searchTerm}'", "Check spelling or try another word.") };
            }

            // Throw exception for other non-success codes (will be caught by the caller)
            response.EnsureSuccessStatusCode();

            // Read and parse the JSON response
            var jsonStream = await response.Content.ReadAsStreamAsync(cancellationToken); 
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            // Use the cancellation token with DeserializeAsync as well
            var entries = await JsonSerializer.DeserializeAsync<List<DictionaryEntry>>(jsonStream, options, cancellationToken);

            if (entries == null || entries.Count == 0)
            {
                 // Should ideally not happen if status code was success, but handle defensively
                return new List<Result> { CreateInfoResult(rawSearch, $"No definitions found for '{searchTerm}'", "API returned empty results despite success status.") };
            }

            // Process entries into Results
            foreach (var entry in entries.Where(e => e != null)) 
            {
                string sourceUrl = entry.SourceUrls?.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s)); 

                // Find the best phonetic representation and audio URL
                var firstPhoneticWithText = entry.Phonetics?.FirstOrDefault(p => p != null && !string.IsNullOrWhiteSpace(p.Text));
                string phoneticText = firstPhoneticWithText?.Text ?? entry.Phonetic; 
                string audioUrl = entry.Phonetics?.FirstOrDefault(p => p != null && !string.IsNullOrWhiteSpace(p.Audio))?.Audio;

                string titlePrefix = $"{entry.Word}";
                if (!string.IsNullOrWhiteSpace(phoneticText))
                {
                    titlePrefix += $" [{phoneticText}]";
                }

                foreach (var meaning in entry.Meanings?.Where(m => m != null) ?? Enumerable.Empty<Meaning>()) 
                {
                    string partOfSpeech = meaning.PartOfSpeech ?? "unknown"; 
                    string titleWithPOS = $"{titlePrefix} ({partOfSpeech})";

                    // 1. Add Definitions
                    foreach (var definition in meaning.Definitions?.Where(d => d != null && !string.IsNullOrWhiteSpace(d.Definition)) ?? Enumerable.Empty<DefinitionItem>()) 
                    {
                        var contextData = new ResultContext
                        {
                            TextToCopy = definition.Definition,
                            AudioUrl = audioUrl, 
                            SourceUrl = sourceUrl,
                            Word = entry.Word
                        };
                        results.Add(new Result
                        {
                            QueryTextDisplay = rawSearch,
                            IcoPath = _iconDefinitionPath,
                            Title = titleWithPOS,
                            SubTitle = definition.Definition,
                            ToolTipData = new ToolTipData(titleWithPOS, definition.Definition),
                            Action = ctx => CopyToClipboard(contextData.TextToCopy), 
                            ContextData = contextData,
                            Score = 100 
                        });

                        // 2. Add Examples for this Definition
                        if (!string.IsNullOrWhiteSpace(definition.Example))
                        {
                            var exampleContext = new ResultContext { TextToCopy = definition.Example, SourceUrl = sourceUrl, Word = entry.Word };
                            results.Add(new Result
                            {
                                QueryTextDisplay = rawSearch,
                                IcoPath = _iconExamplePath,
                                Title = $"Example ({partOfSpeech})",
                                SubTitle = definition.Example,
                                ToolTipData = new ToolTipData("Example", definition.Example),
                                Action = ctx => CopyToClipboard(exampleContext.TextToCopy), 
                                ContextData = exampleContext,
                                Score = 90 
                            });
                        }
                    }

                    // 3. Add Synonyms for this Meaning
                    var validSynonyms = meaning.Synonyms?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    if (validSynonyms?.Any() ?? false)
                    {
                        string synonymsText = string.Join(", ", validSynonyms);
                        var synonymContext = new ResultContext { TextToCopy = synonymsText, SourceUrl = sourceUrl, Word = entry.Word };
                        results.Add(new Result
                        {
                            QueryTextDisplay = rawSearch,
                            IcoPath = _iconSynonymPath,
                            Title = $"Synonyms ({partOfSpeech})",
                            SubTitle = synonymsText,
                            ToolTipData = new ToolTipData("Synonyms", synonymsText),
                            Action = ctx => CopyToClipboard(synonymContext.TextToCopy), 
                            ContextData = synonymContext,
                            Score = 80 
                        });
                    }

                    // 4. Add Antonyms for this Meaning
                    var validAntonyms = meaning.Antonyms?.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
                    if (validAntonyms?.Any() ?? false)
                    {
                        string antonymsText = string.Join(", ", validAntonyms);
                        var antonymContext = new ResultContext { TextToCopy = antonymsText, SourceUrl = sourceUrl, Word = entry.Word };
                        results.Add(new Result
                        {
                            QueryTextDisplay = rawSearch,
                            IcoPath = _iconAntonymPath,
                            Title = $"Antonyms ({partOfSpeech})",
                            SubTitle = antonymsText,
                            ToolTipData = new ToolTipData("Antonyms", antonymsText),
                            Action = ctx => CopyToClipboard(antonymContext.TextToCopy), 
                            ContextData = antonymContext,
                            Score = 75 
                        });
                    }
                }
            }

            // Fallback if no processable definitions found despite successful API call
            if (results.Count == 0)
            {
                 return new List<Result> { CreateInfoResult(rawSearch, $"No definitions found for '{searchTerm}'", "API returned data, but no definitions could be processed.") };
            }

            return results;
        }


        // --- Context Menu ---
            public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
            {
                // Ensure ContextData is the correct type
                if (!(selectedResult.ContextData is ResultContext context))
                {
                    return new List<ContextMenuResult>(); 
                }

                var menuItems = new List<ContextMenuResult>();

                // 1. Copy Item (Definition, Example, Synonym, Antonym)
                if (!string.IsNullOrWhiteSpace(context.TextToCopy))
                {
                    menuItems.Add(new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = $"Copy: {Truncate(context.TextToCopy)} (Ctrl+C)",
                        FontFamily = "Segoe MDL2 Assets", 
                        Glyph = "\xE8C8", 
                        AcceleratorKey = Key.C,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ => CopyToClipboard(context.TextToCopy)
                    });
                }

                // 2. Play Pronunciation (if available)
                if (Uri.IsWellFormedUriString(context.AudioUrl, UriKind.Absolute))
                {
                    menuItems.Add(new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Play Pronunciation",
                        FontFamily = "Segoe MDL2 Assets", 
                        Glyph = "\xE768", 
                        Action = actionContext => PlayAudio(context.AudioUrl)
                    });
                }

                // 3. Open Source URL (if available)
                if (Uri.IsWellFormedUriString(context.SourceUrl, UriKind.Absolute))
                {
                     menuItems.Add(new ContextMenuResult
                     {
                         PluginName = Name,
                         Title = "Open Source URL in Browser",
                         FontFamily = "Segoe MDL2 Assets", 
                         Glyph = "\xE774", 
                         Action = _ => OpenUrl(context.SourceUrl)
                     });
                }

                // 4. Search for the original word again (useful if selected synonym/antonym/example)
                bool isAuxiliaryResult = selectedResult.Title.Contains("Synonyms") ||
                                          selectedResult.Title.Contains("Antonyms") ||
                                          selectedResult.Title.Contains("Example");

                if (!string.IsNullOrWhiteSpace(context.Word) && isAuxiliaryResult)
                {
                    menuItems.Add(new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = $"Define '{context.Word}'",
                        FontFamily = "Segoe MDL2 Assets", 
                        Glyph = "\xE721", 
                        Action = _ =>
                        {
                            if (_context?.CurrentPluginMetadata != null)
                            {
                                _context.API.ChangeQuery($"{_context.CurrentPluginMetadata.ActionKeyword} {context.Word}", true);
                                return true;
                            }
                            return false;
                        }
                    });
                }

                return menuItems;
            }

        // --- Helper Methods ---

        private bool CopyToClipboard(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            try
            {
                // Run on STA thread required for clipboard operations in WPF/WinForms contexts
                bool success = false;
                Thread staThread = new Thread(() =>
                {
                    try
                    {
                        Clipboard.SetDataObject(text, true); 
                        success = true;
                    }
                    catch (Exception ex)
                    {
                         // Log exception from the STA thread
                         Debug.WriteLine($"[Definition Plugin] STA Clipboard Error: {ex.ToString()}");
                        success = false;
                    }
                });
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join(); 
                return success; 
            }
            catch (Exception ex)
            {
                // Log exception 
                Debug.WriteLine($"[Definition Plugin] Clipboard Thread Error: {ex.ToString()}");
                return false; 
            }
        }

         private bool PlayAudio(string url)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) return false;

                // Log playback attempt
                Debug.WriteLine($"[Definition Plugin] Playing audio from: {url}");

                // Ensure MediaPlayer runs on the correct thread
                _mediaPlayer.Open(new Uri(url));
                _mediaPlayer.Play();

                return false; 
            }
            catch (Exception ex)
            {
                 // Use Debug.WriteLine for errors
                 Debug.WriteLine($"[Definition Plugin] PlayAudio Error for {url}: {ex.ToString()}");
                return false; 
            }
        }

         private bool OpenUrl(string url)
        {
             try
             {
                 if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) return false;

                 // Log URL opening
                 Debug.WriteLine($"[Definition Plugin] Opening URL: {url}");

                 // Use ShellExecute to open the default browser
                 Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                 return true; 
             }
             catch (Exception ex)
             {
                 // Use Debug.WriteLine for errors
                 Debug.WriteLine($"[Definition Plugin] OpenUrl Error for {url}: {ex.ToString()}");
                 return false; 
             }
        }

        private string Truncate(string text, int maxLength = 30)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }

        // Helper methods to create standard result types
        private Result CreateInfoResult(string rawSearch, string title, string subTitle) => new()
        {
            QueryTextDisplay = rawSearch,
            IcoPath = _iconInfoPath, 
            Title = title,
            SubTitle = subTitle,
            Action = _ => false, 
            ContextData = null 
        };

        private Result CreateErrorResult(string rawSearch, string title, string subTitle) => new()
        {
            QueryTextDisplay = rawSearch,
            IcoPath = _iconErrorPath, 
            Title = title,
            SubTitle = subTitle,
            Action = _ => false, 
            ContextData = null 
        };


        // --- JSON Models ---
        // These classes represent the structure of the JSON response from the API

        private class DictionaryEntry
        {
            public string Word { get; set; }
            public string Phonetic { get; set; } 
            public List<Phonetic> Phonetics { get; set; } = new();
            public List<Meaning> Meanings { get; set; } = new();
            public LicenseInfo License { get; set; }
            public List<string> SourceUrls { get; set; } = new();
        }

        private class Phonetic
        {
            public string Text { get; set; }
            public string Audio { get; set; } 
            public string SourceUrl { get; set; }
            public LicenseInfo License { get; set; }
        }

        private class Meaning
        {
            public string PartOfSpeech { get; set; }
            public List<DefinitionItem> Definitions { get; set; } = new();
            public List<string> Synonyms { get; set; } = new();
            public List<string> Antonyms { get; set; } = new();
        }

        private class DefinitionItem
        {
            public string Definition { get; set; }
            public string Example { get; set; }
        }

        private class LicenseInfo
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        // --- Context Data Class ---
        // Helper class to store multiple pieces of data for context menus
        private class ResultContext
        {
            public string TextToCopy { get; set; }
            public string AudioUrl { get; set; }
            public string SourceUrl { get; set; }
            public string Word { get; set; } 
        }
    }
}