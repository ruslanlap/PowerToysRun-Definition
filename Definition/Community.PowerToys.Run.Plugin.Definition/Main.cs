using ManagedCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;      // for MediaPlayer
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Definition
{
    public class Main : IPlugin, IDelayedExecutionPlugin, IContextMenu, IDisposable
    {
        public static string PluginID => "AF6979212B9D429489F115EE3390D608";
        public string Name            => "Definition";
        public string Description     => "Lookup word definitions, phonetics, and synonyms";

        private PluginInitContext _context;
        private string _iconPath;
        private bool _disposed;
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, List<Result>> _cache = new();
        private readonly MediaPlayer _mediaPlayer = new();

        public Main()
        {
            _httpClient = new HttpClient {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(_context.API.GetCurrentTheme());
        }

        public List<Result> Query(Query query) =>
            Query(query, delayedExecution: false);

        public List<Result> Query(Query query, bool delayedExecution)
        {
            var raw    = query.Search ?? string.Empty;
            var search = raw.Trim();
            var results = new List<Result>();

            if (string.IsNullOrEmpty(search))
            {
                results.Add(new Result {
                    QueryTextDisplay = raw,
                    IcoPath           = _iconPath,
                    Title             = "Dictionary",
                    SubTitle          = "Type a word…",
                    Action            = _ => false,
                    ContextData       = string.Empty
                });
                return results;
            }

            if (_cache.TryGetValue(search, out var cached))
                return cached;

            if (!delayedExecution)
            {
                results.Add(new Result {
                    QueryTextDisplay = raw,
                    IcoPath           = _iconPath,
                    Title             = "Loading…",
                    SubTitle          = $"Looking up '{search}'",
                    Action            = _ => false,
                    ContextData       = string.Empty
                });
                return results;
            }

            try
            {
                var url      = $"https://api.dictionaryapi.dev/api/v2/entries/en/{Uri.EscapeDataString(search)}";
                var response = _httpClient.GetAsync(url).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    results.Add(MakeNoDefResult(raw, search));
                    _cache[search] = results;
                    return results;
                }

                var json     = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var options  = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var entries  = JsonSerializer.Deserialize<List<DictionaryEntry>>(json, options);

                if (entries == null || entries.Count == 0)
                {
                    results.Add(MakeNoDefResult(raw, search));
                    _cache[search] = results;
                    return results;
                }

                foreach (var entry in entries)
                {
                    var phonText = string.IsNullOrEmpty(entry.Phonetic)
                        ? ""
                        : $" [{entry.Phonetic}]";

                    var audioUrl = entry.Phonetics
                        .FirstOrDefault(p => !string.IsNullOrEmpty(p.Audio))
                        ?.Audio;

                    foreach (var meaning in entry.Meanings)
                    foreach (var def in meaning.Definitions)
                    {
                        var text = def.Definition;
                        results.Add(new Result {
                            QueryTextDisplay = raw,
                            IcoPath           = _iconPath,
                            Title             = $"{entry.Word}{phonText} ({meaning.PartOfSpeech})",
                            SubTitle          = text,
                            ToolTipData       = new ToolTipData(entry.Word, text),
                            Action            = _ => {
                                Clipboard.SetDataObject(text);
                                return true;
                            },
                            ContextData       = audioUrl ?? text
                        });
                    }
                }

                if (_cache.Count > 100) _cache.Clear();
                _cache[search] = results;
            }
            catch (TaskCanceledException)
            {
                results.Clear();
                results.Add(new Result {
                    QueryTextDisplay = raw,
                    IcoPath           = _iconPath,
                    Title             = "Request timed out",
                    SubTitle          = "Try again or check your connection",
                    Action            = _ => false,
                    ContextData       = string.Empty
                });
            }
            catch
            {
                results.Clear();
                results.Add(new Result {
                    QueryTextDisplay = raw,
                    IcoPath           = _iconPath,
                    Title             = "Error looking up definition",
                    SubTitle          = "Please try again later",
                    Action            = _ => false,
                    ContextData       = string.Empty
                });
            }

            return results;
        }

        public List<ContextMenuResult> LoadContextMenus(Result selected)
        {
            var data = selected.ContextData as string;
            if (string.IsNullOrEmpty(data))
                return new List<ContextMenuResult>();

            var menus = new List<ContextMenuResult> {
                new ContextMenuResult {
                    PluginName           = Name,
                    Title                = "Copy Definition (Ctrl+C)",
                    FontFamily           = "Segoe MDL2 Assets",
                    Glyph                = "\xE8C8",
                    AcceleratorKey       = Key.C,
                    AcceleratorModifiers = ModifierKeys.Control,
                    Action               = _ => {
                        Clipboard.SetDataObject(selected.SubTitle);
                        return true;
                    }
                }
            };

            // Play icon in place, keep window open
            if (Uri.IsWellFormedUriString(data, UriKind.Absolute))
            {
                menus.Add(new ContextMenuResult {
                    PluginName = Name,
                    FontFamily = "Segoe MDL2 Assets",
                    Glyph      = "\xE768",       // Play icon
                    Title      = "Play pronunciation",
                    Action     = _ => {
                        _mediaPlayer.Open(new Uri(data));
                        _mediaPlayer.Play();
                        return false;            // <-- keep plugin open
                    }
                });
            }

            return menus;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing) return;
            if (_context?.API != null)
                _context.API.ThemeChanged -= OnThemeChanged;
            _httpClient.Dispose();
            _mediaPlayer.Close();
            _disposed = true;
        }

        private void UpdateIconPath(Theme theme) =>
            _iconPath = (theme == Theme.Light || theme == Theme.HighContrastWhite)
                ? "Images/definition.light.png"
                : "Images/definition.dark.png";

        private void OnThemeChanged(Theme _, Theme newTheme) =>
            UpdateIconPath(newTheme);

        private static Result MakeNoDefResult(string raw, string search) => new() {
            QueryTextDisplay = raw,
            IcoPath           = "Images/definition.light.png",
            Title             = $"No definitions found for '{search}'",
            SubTitle          = "Check spelling or try another word",
            Action            = _ => false,
            ContextData       = string.Empty
        };

        // JSON models
        private class DictionaryEntry {
            public string Word { get; set; }
            public string Phonetic { get; set; }
            public List<Phonetic> Phonetics { get; set; } = new();
            public List<Meaning> Meanings { get; set; } = new();
        }
        private class Phonetic {
            public string Text  { get; set; }
            public string Audio { get; set; }
        }
        private class Meaning {
            public string PartOfSpeech { get; set; }
            public List<DefinitionItem> Definitions { get; set; } = new();
        }
        private class DefinitionItem {
            public string Definition { get; set; }
        }
    }
}
