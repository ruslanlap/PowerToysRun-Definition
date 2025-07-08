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

        private static HttpClient _httpClient;
        private static HttpClient HttpClient 
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient
                    {
                        Timeout = TimeSpan.FromSeconds(ConfigurationManager.Configuration.HttpTimeoutSeconds)
                    };
                }
                else
                {
                    // Update timeout if configuration changed
                    _httpClient.Timeout = TimeSpan.FromSeconds(ConfigurationManager.Configuration.HttpTimeoutSeconds);
                }
                return _httpClient;
            }
        }

        private LRUCache _cache;
        private readonly AudioManager _audioManager;
        private CancellationTokenSource _cancellationTokenSource;
        #endregion

        #region Initialization
        public Main()
        {
            _iconManager = new IconManager();
            _audioManager = new AudioManager();
        }

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.API.ThemeChanged += OnThemeChanged;

            _pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _iconManager.Initialize(_pluginDirectory, _context.API.GetCurrentTheme());
            
            // Initialize cache with current configuration
            _cache = new LRUCache(ConfigurationManager.Configuration.CacheMaxSize);

            LogHelper.WriteLog($"Initialized. Directory: {_pluginDirectory}");
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
            
            // Update cache size if needed
            if (_cache != null && _cache.Capacity != ConfigurationManager.Configuration.CacheMaxSize)
            {
                _cache = new LRUCache(ConfigurationManager.Configuration.CacheMaxSize);
                LogHelper.WriteLog($"Cache resized to {ConfigurationManager.Configuration.CacheMaxSize} entries");
            }
            
            var rawSearch = query.Search ?? string.Empty;
            var searchTerm = rawSearch.Trim().ToLowerInvariant();

            CancelPreviousRequest();

            // Handle empty query
            if (string.IsNullOrWhiteSpace(searchTerm))
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
                return new List<Result> { CreateInfoResult(rawSearch, SearchingMessage, $"Searching for '{searchTerm}'") };
            }

            // Perform actual API call
            return ExecuteDelayedQuery(searchTerm, rawSearch);
        }

        private void CancelPreviousRequest()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private bool TryGetCachedResults(string searchTerm, string rawSearch, out List<Result> results)
        {
            if (_cache?.TryGetValue(searchTerm, out var cacheItem) == true)
            {
                results = cacheItem.Results.Select(r => r.Clone(rawSearch)).ToList();
                return true;
            }

            results = null;
            return false;
        }

        private List<Result> ExecuteDelayedQuery(string searchTerm, string rawSearch)
        {
            try
            {
                // Use Task.Run to avoid blocking the UI thread
                var task = Task.Run(async () => await FetchAndProcessResultsAsync(searchTerm, rawSearch, _cancellationTokenSource.Token));
                var results = task.ConfigureAwait(false).GetAwaiter().GetResult();

                CacheResults(searchTerm, results);
                return results;
            }
            catch (OperationCanceledException)
            {
                return new List<Result> { CreateInfoResult(rawSearch, "Searching...", "...") };
            }
            catch (HttpRequestException ex)
            {
                LogHelper.WriteError($"HTTP error for '{searchTerm}': {ex.Message}");
                var errorMessage = ex.Data.Contains("StatusCode") 
                    ? $"Could not reach dictionary service. ({ex.Data["StatusCode"]})"
                    : "Could not reach dictionary service. Check connection.";
                return new List<Result> { CreateErrorResult(rawSearch, NetworkErrorTitle, errorMessage) };
            }
            catch (JsonException ex)
            {
                LogHelper.WriteError($"JSON parsing failed for '{searchTerm}': {ex.Message}");
                return new List<Result> { CreateErrorResult(rawSearch, ApiErrorTitle, "Failed to parse response from dictionary service.") };
            }
            catch (Exception ex)
            {
                LogHelper.WriteError($"Unexpected error for '{searchTerm}': {ex}");
                return new List<Result> { CreateErrorResult(rawSearch, UnexpectedErrorTitle, "An unexpected error occurred.") };
            }
        }

        private void CacheResults(string searchTerm, List<Result> results)
        {
            if (_cache != null && results.Any() && results.First().ContextData is ResultContext)
            {
                var cacheItem = new CacheItem(results, DateTime.UtcNow, searchTerm);
                _cache.Set(searchTerm, cacheItem);
            }
        }
        #endregion

        #region API Communication
        private async Task<List<Result>> FetchAndProcessResultsAsync(string searchTerm, string rawSearch, CancellationToken cancellationToken)
        {
            return await RetryHelper.RetryAsync(async () =>
            {
                var requestUrl = $"{ConfigurationManager.Configuration.ApiEndpoint}{Uri.EscapeDataString(searchTerm)}";

                using var response = await HttpClient.GetAsync(requestUrl, cancellationToken);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new List<Result> { CreateInfoResult(rawSearch, $"No definitions found for '{searchTerm}'", "Check spelling or try another word.") };
                }

                // Store status code for retry logic
                if (!response.IsSuccessStatusCode)
                {
                    var httpEx = new HttpRequestException($"HTTP {(int)response.StatusCode} {response.StatusCode}");
                    httpEx.Data["StatusCode"] = response.StatusCode;
                    throw httpEx;
                }

                await using var jsonStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var entries = await JsonSerializer.DeserializeAsync<List<DictionaryEntry>>(jsonStream, options, cancellationToken);

                if (entries?.Any() != true)
                {
                    return new List<Result> { CreateInfoResult(rawSearch, $"No definitions found for '{searchTerm}'", "API returned empty results.") };
                }

                return ProcessDictionaryEntries(entries, rawSearch);
            }, cancellationToken, 3, $"Dictionary lookup for '{searchTerm}'");
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

            return results.Any() 
                ? results 
                : new List<Result> { CreateInfoResult(rawSearch, "No definitions found", "No processable definitions in API response.") };
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