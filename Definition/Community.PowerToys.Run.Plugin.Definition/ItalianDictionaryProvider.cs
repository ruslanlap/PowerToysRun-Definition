using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class ItalianDictionaryProvider : IDictionaryProvider
    {
        private readonly HttpClient _httpClient;
        private const string WiktionaryApiBase = "https://it.wiktionary.org/w/api.php";

        public string LanguageCode => "it";
        public string DisplayName => "Italiano (Wikizionario)";

        private static readonly string[] PartOfSpeechKeywords =
        {
            "sostantivo",
            "nome",
            "verbo",
            "aggettivo",
            "avverbio",
            "pronome",
            "preposizione",
            "congiunzione",
            "interiezione",
            "articolo",
            "locuzione"
        };

        public ItalianDictionaryProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token)
        {
            try
            {
                return await LookupWiktionaryAsync(word, token);
            }
            catch (Exception ex) when (!token.IsCancellationRequested)
            {
                Debug.WriteLine($"[ItalianProvider] Wiktionary failed for '{word}': {ex.GetType().Name}: {ex.Message}");
                return new List<DictionaryEntry>();
            }
        }

        private async Task<List<DictionaryEntry>> LookupWiktionaryAsync(string word, CancellationToken token)
        {
            var encodedWord = Uri.EscapeDataString(word);
            var url = $"{WiktionaryApiBase}?action=parse&page={encodedWord}&prop=wikitext&format=json&redirects=1";

            using var response = await _httpClient.GetAsync(url, token);
            if (!response.IsSuccessStatusCode)
                return new List<DictionaryEntry>();

            var json = await response.Content.ReadAsStringAsync(token);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("error", out _))
                return new List<DictionaryEntry>();

            if (!root.TryGetProperty("parse", out var parse)
                || !parse.TryGetProperty("wikitext", out var wikitext)
                || !wikitext.TryGetProperty("*", out var content))
            {
                return new List<DictionaryEntry>();
            }

            var text = content.GetString();
            if (string.IsNullOrWhiteSpace(text))
                return new List<DictionaryEntry>();

            return ParseWikitext(word, text);
        }

        private static List<DictionaryEntry> ParseWikitext(string word, string wikitext)
        {
            var section = ExtractItalianSection(wikitext);
            if (string.IsNullOrWhiteSpace(section))
                return new List<DictionaryEntry>();

            var meanings = ExtractMeanings(section);
            if (meanings.Count == 0)
                return new List<DictionaryEntry>();

            var entry = new DictionaryEntry
            {
                Word = word,
                SourceUrls = new List<string>
                {
                    $"https://it.wiktionary.org/wiki/{Uri.EscapeDataString(word)}"
                }
            };

            entry.Meanings.AddRange(meanings);
            return new List<DictionaryEntry> { entry };
        }

        private static string ExtractItalianSection(string wikitext)
        {
            var header = new Regex(
                @"^==\s*\{\{lingua\|it\}\}\s*==\s*$|^==\s*Italiano\s*==\s*$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            var match = header.Match(wikitext);

            if (!match.Success)
            {
                // it.wiktionary often has Italian-only pages.
                return wikitext.Contains("definizione", StringComparison.OrdinalIgnoreCase)
                    || wikitext.Contains("significato", StringComparison.OrdinalIgnoreCase)
                    ? wikitext
                    : null;
            }

            var start = match.Index + match.Length;

            var nextLangHeader = new Regex(
                @"^==\s*(\{\{lingua\|[a-z\-]+\}\}|[A-ZÀ-Ü][^=]+)\s*==\s*$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            var next = nextLangHeader.Match(wikitext, start);
            var end = next.Success ? next.Index : wikitext.Length;

            return wikitext.Substring(start, end - start);
        }

        private static List<Meaning> ExtractMeanings(string section)
        {
            var meanings = new List<Meaning>();

            var headingRegex = new Regex(
                @"^={3,6}\s*(?<title>[^=]+?)\s*={3,6}$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            string currentPartOfSpeech = null;
            List<DefinitionItem> currentDefinitions = null;

            void Commit()
            {
                if (currentDefinitions != null && currentDefinitions.Count > 0)
                {
                    meanings.Add(new Meaning
                    {
                        PartOfSpeech = currentPartOfSpeech,
                        Definitions = currentDefinitions.Take(5).ToList()
                    });
                }
            }

            foreach (var rawLine in section.Split('\n'))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var headingMatch = headingRegex.Match(line);
                if (headingMatch.Success)
                {
                    Commit();

                    var heading = CleanWikitext(headingMatch.Groups["title"].Value).ToLowerInvariant();

                    if (IsPartOfSpeech(heading))
                    {
                        currentPartOfSpeech = heading;
                        currentDefinitions = new List<DefinitionItem>();
                    }
                    else
                    {
                        currentPartOfSpeech = null;
                        currentDefinitions = null;
                    }

                    continue;
                }

                if (currentDefinitions == null)
                    continue;

                if (!line.StartsWith("#")
                    || line.StartsWith("#*")
                    || line.StartsWith("#:"))
                {
                    continue;
                }

                var definitionText = Regex.Replace(line, @"^#+\s*", "");
                var cleanDefinition = CleanWikitext(definitionText);

                if (!string.IsNullOrWhiteSpace(cleanDefinition))
                {
                    currentDefinitions.Add(new DefinitionItem
                    {
                        Definition = cleanDefinition.Length > 250
                            ? cleanDefinition.Substring(0, 247) + "..."
                            : cleanDefinition
                    });
                }
            }

            Commit();
            return meanings;
        }

        private static bool IsPartOfSpeech(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return PartOfSpeechKeywords.Any(keyword =>
                text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private static string CleanWikitext(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = Regex.Replace(text, @"<!--.*?-->", "", RegexOptions.Singleline);
            text = Regex.Replace(text, @"<ref[^>]*>.*?</ref>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<[^>]+>", "");

            text = Regex.Replace(text, @"\[\[([^|\]]*\|)?([^\]]+)\]\]", "$2");
            text = Regex.Replace(text, @"\{\{term\|([^|}]+)(?:\|[^}]*)?\}\}", "$1", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"\{\{linkp\|([^|}]+)(?:\|[^}]*)?\}\}", "$1", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"\{\{[^}]+\}\}", "");

            text = text.Replace("'''", "").Replace("''", "");
            text = WebUtility.HtmlDecode(text);
            text = Regex.Replace(text, @"\s+", " ").Trim();

            return text;
        }
    }
}
