using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using HtmlAgilityPack;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class FrenchDictionaryProvider : IDictionaryProvider
    {
        private readonly HttpClient _httpClient;
        private const string CollinsDictionaryBase = "https://www.collinsdictionary.com/dictionary/french-english/";
        private const string WiktionaryApiBase = "https://fr.wiktionary.org/w/api.php";
        public string LanguageCode => "fr";
        public string DisplayName => "Français (Collins + Wiktionnaire)";

        private static readonly string[] CollinsPartOfSpeechKeywords =
        {
            "noun",
            "verb",
            "adjective",
            "adverb",
            "pronoun",
            "preposition",
            "conjunction",
            "interjection",
            "determiner",
            "article",
            "exclamation",
            "phrase",
            "auxiliary"
        };

        private static readonly string[] PartOfSpeechKeywords =
        {
            "nom",
            "verbe",
            "adjectif",
            "adverbe",
            "pronom",
            "préposition",
            "conjonction",
            "interjection",
            "déterminant",
            "article",
            "particule",
            "locution",
            "onomatop"
        };

        public FrenchDictionaryProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token)
        {
            var candidates = BuildWordCandidates(word);

            foreach (var candidate in candidates)
            {
                var collinsEntries = await TryLookupCollinsAsync(candidate, token);
                if (collinsEntries.Count > 0)
                {
                    return collinsEntries;
                }

                var wiktionaryEntries = await TryLookupWiktionaryAsync(candidate, token);
                if (wiktionaryEntries.Count > 0)
                {
                    return wiktionaryEntries;
                }
            }

            return new List<DictionaryEntry>();
        }

        private static List<string> BuildWordCandidates(string word)
        {
            var candidates = new List<string>();

            if (!string.IsNullOrWhiteSpace(word))
            {
                candidates.Add(word);
            }

            var normalizedWord = NormalizeFrenchWord(word);
            if (!string.IsNullOrWhiteSpace(normalizedWord)
                && !string.Equals(normalizedWord, word, StringComparison.Ordinal))
            {
                candidates.Add(normalizedWord);
            }

            return candidates.Distinct(StringComparer.Ordinal).ToList();
        }

        private async Task<List<DictionaryEntry>> TryLookupCollinsAsync(string word, CancellationToken token)
        {
            try
            {
                return await LookupCollinsAsync(word, token);
            }
            catch (Exception ex) when (!token.IsCancellationRequested)
            {
                Debug.WriteLine($"[FrenchProvider] Collins lookup failed for '{word}': {ex.GetType().Name}: {ex.Message}");
                return new List<DictionaryEntry>();
            }
        }

        private async Task<List<DictionaryEntry>> LookupCollinsAsync(string word, CancellationToken token)
        {
            var requestUrl = $"{CollinsDictionaryBase}{Uri.EscapeDataString(word)}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
            request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9,fr;q=0.8");

            using var response = await _httpClient.SendAsync(request, token);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<DictionaryEntry>();
            }

            if (!response.IsSuccessStatusCode)
            {
                return new List<DictionaryEntry>();
            }

            var html = await response.Content.ReadAsStringAsync(token);
            return ParseCollinsHtml(word, requestUrl, html);
        }

        private static List<DictionaryEntry> ParseCollinsHtml(string queryWord, string sourceUrl, string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return new List<DictionaryEntry>();
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var contentNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'content') and contains(@class, 'definitions')]")
                ?? doc.DocumentNode.SelectSingleNode("//main")
                ?? doc.DocumentNode;

            var meanings = ExtractCollinsMeanings(contentNode, queryWord);
            if (meanings.Count == 0)
            {
                return new List<DictionaryEntry>();
            }

            var entry = new DictionaryEntry
            {
                Word = ExtractCollinsWord(contentNode, queryWord),
                SourceUrls = new List<string> { sourceUrl }
            };

            entry.Meanings.AddRange(meanings);
            return new List<DictionaryEntry> { entry };
        }

        private static string ExtractCollinsWord(HtmlNode contentNode, string fallbackWord)
        {
            var titleNode = contentNode.SelectSingleNode(".//h1");
            var titleText = NormalizeHtmlText(titleNode?.InnerText);
            if (!string.IsNullOrWhiteSpace(titleText))
            {
                var quotedWordMatch = Regex.Match(titleText, "['‘“](?<word>[^'’”]+)['’”]", RegexOptions.IgnoreCase);
                if (quotedWordMatch.Success)
                {
                    return quotedWordMatch.Groups["word"].Value.Trim();
                }
            }

            var headingNodes = contentNode.SelectNodes(".//h2");
            if (headingNodes != null)
            {
                foreach (var headingNode in headingNodes)
                {
                    var headingText = NormalizeHtmlText(headingNode.InnerText);
                    if (IsLikelyHeadword(headingText))
                    {
                        return headingText;
                    }
                }
            }

            return fallbackWord;
        }

        private static bool IsLikelyHeadword(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length > 40)
            {
                return false;
            }

            if (text.StartsWith("Examples of", StringComparison.OrdinalIgnoreCase)
                || text.Contains("translation", StringComparison.OrdinalIgnoreCase)
                || text.Contains("Collins", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return Regex.IsMatch(text, @"^[\p{L}\p{M}\-\'\u2019\s]+$");
        }

        private static List<Meaning> ExtractCollinsMeanings(HtmlNode contentNode, string lookupWord)
        {
            var meaningMap = new Dictionary<string, List<DefinitionItem>>(StringComparer.OrdinalIgnoreCase);
            var totalDefinitions = 0;

            var definitionNodes = contentNode.SelectNodes(".//div[contains(@class,'hom') or contains(@class,'sense')]//span[contains(@class,'def') or contains(@class,'quote')]")
                ?? contentNode.SelectNodes(".//span[contains(@class,'def') or contains(@class,'quote')]");

            if (definitionNodes == null || definitionNodes.Count == 0)
            {
                return new List<Meaning>();
            }

            foreach (var definitionNode in definitionNodes)
            {
                if (IsInsideExamplesArea(definitionNode))
                {
                    continue;
                }

                var definitionText = NormalizeHtmlText(definitionNode.InnerText);
                if (!IsUsableCollinsDefinition(definitionText, lookupWord))
                {
                    continue;
                }

                var partOfSpeech = ExtractCollinsPartOfSpeech(definitionNode);
                if (!meaningMap.TryGetValue(partOfSpeech, out var definitions))
                {
                    definitions = new List<DefinitionItem>();
                    meaningMap[partOfSpeech] = definitions;
                }

                if (definitions.Any(d => string.Equals(d.Definition, definitionText, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                definitions.Add(new DefinitionItem { Definition = definitionText });
                totalDefinitions++;

                if (totalDefinitions >= 12)
                {
                    break;
                }
            }

            return meaningMap
                .Where(kvp => kvp.Value.Count > 0)
                .Select(kvp => new Meaning
                {
                    PartOfSpeech = kvp.Key,
                    Definitions = kvp.Value.Take(5).ToList()
                })
                .ToList();
        }

        private static bool IsInsideExamplesArea(HtmlNode node)
        {
            foreach (var ancestor in node.AncestorsAndSelf())
            {
                var className = ancestor.GetAttributeValue("class", string.Empty);
                if (className.Contains("example", StringComparison.OrdinalIgnoreCase)
                    || className.Contains("corpus", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                var id = ancestor.GetAttributeValue("id", string.Empty);
                if (id.Contains("example", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ExtractCollinsPartOfSpeech(HtmlNode node)
        {
            foreach (var ancestor in node.AncestorsAndSelf().Take(8))
            {
                var posNode = ancestor.SelectSingleNode(".//span[contains(@class,'pos')]");
                var posText = NormalizeHtmlText(posNode?.InnerText);
                if (IsLikelyPartOfSpeech(posText))
                {
                    return posText;
                }
            }

            return "translation";
        }

        private static bool IsLikelyPartOfSpeech(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length > 30)
            {
                return false;
            }

            return CollinsPartOfSpeechKeywords.Any(keyword =>
                text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsUsableCollinsDefinition(string text, string lookupWord)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length < 2 || text.Length > 220)
            {
                return false;
            }

            if (!text.Any(char.IsLetter)
                || text.StartsWith("Word forms", StringComparison.OrdinalIgnoreCase)
                || text.StartsWith("Examples of", StringComparison.OrdinalIgnoreCase)
                || text.StartsWith("English translation of", StringComparison.OrdinalIgnoreCase)
                || text.StartsWith("French translation of", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var normalizedLookup = NormalizeFrenchWord(lookupWord);
            var normalizedText = NormalizeFrenchWord(text);
            return !string.Equals(normalizedLookup, normalizedText, StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeHtmlText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            text = HtmlEntity.DeEntitize(text);
            text = Regex.Replace(text, @"\s+", " ").Trim();

            return text;
        }

        private async Task<List<DictionaryEntry>> TryLookupWiktionaryAsync(string word, CancellationToken token)
        {
            try
            {
                return await LookupWiktionaryAsync(word, token);
            }
            catch (Exception ex) when (!token.IsCancellationRequested)
            {
                Debug.WriteLine($"[FrenchProvider] Wiktionary fallback failed for '{word}': {ex.GetType().Name}: {ex.Message}");
                return new List<DictionaryEntry>();
            }
        }

        private async Task<List<DictionaryEntry>> LookupWiktionaryAsync(string word, CancellationToken token)
        {
            var encodedWord = Uri.EscapeDataString(word);
            var requestUrl = $"{WiktionaryApiBase}?action=parse&page={encodedWord}&prop=wikitext&format=json&redirects=1";

            using var response = await _httpClient.GetAsync(requestUrl, token);
            if (!response.IsSuccessStatusCode)
            {
                return new List<DictionaryEntry>();
            }

            var json = await response.Content.ReadAsStringAsync(token);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("error", out _))
            {
                return new List<DictionaryEntry>();
            }

            if (!root.TryGetProperty("parse", out var parse)
                || !parse.TryGetProperty("wikitext", out var wikitext)
                || !wikitext.TryGetProperty("*", out var wikitextContent))
            {
                return new List<DictionaryEntry>();
            }

            var text = wikitextContent.GetString();
            if (string.IsNullOrWhiteSpace(text))
            {
                return new List<DictionaryEntry>();
            }

            return ParseWiktionaryWikitext(word, text);
        }

        private static List<DictionaryEntry> ParseWiktionaryWikitext(string word, string wikitext)
        {
            var frenchSection = ExtractFrenchSection(wikitext);
            if (string.IsNullOrWhiteSpace(frenchSection))
            {
                return new List<DictionaryEntry>();
            }

            var meanings = ExtractMeanings(frenchSection);
            if (meanings.Count == 0)
            {
                return new List<DictionaryEntry>();
            }

            var entry = new DictionaryEntry
            {
                Word = word,
                SourceUrls = new List<string> { $"https://fr.wiktionary.org/wiki/{Uri.EscapeDataString(word)}" }
            };

            entry.Meanings.AddRange(meanings);
            return new List<DictionaryEntry> { entry };
        }

        private static string ExtractFrenchSection(string wikitext)
        {
            var frenchLanguageHeader = new Regex(
                @"^==\s*\{\{langue\|fr(?:\|[^}]*)?\}\}\s*==\s*$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            var headerMatch = frenchLanguageHeader.Match(wikitext);
            if (!headerMatch.Success)
            {
                return null;
            }

            var sectionStart = headerMatch.Index + headerMatch.Length;
            var anyLanguageHeader = new Regex(
                @"^==\s*\{\{langue\|[a-z\-]+(?:\|[^}]*)?\}\}\s*==\s*$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            var nextLanguageMatch = anyLanguageHeader.Match(wikitext, sectionStart);
            var sectionEnd = nextLanguageMatch.Success ? nextLanguageMatch.Index : wikitext.Length;

            return wikitext.Substring(sectionStart, sectionEnd - sectionStart);
        }

        private static List<Meaning> ExtractMeanings(string frenchSection)
        {
            var meanings = new List<Meaning>();
            var headingRegex = new Regex(
                @"^={3,6}\s*\{\{S\|([^|}]+)(?:\|[^}]*)?\}\}\s*={3,6}$",
                RegexOptions.IgnoreCase);

            string currentPartOfSpeech = null;
            List<DefinitionItem> currentDefinitions = null;

            void CommitMeaning()
            {
                if (currentDefinitions != null && currentDefinitions.Count > 0)
                {
                    meanings.Add(new Meaning
                    {
                        PartOfSpeech = currentPartOfSpeech,
                        Definitions = currentDefinitions
                    });
                }
            }

            foreach (var rawLine in frenchSection.Split('\n'))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var headingMatch = headingRegex.Match(line);
                if (headingMatch.Success)
                {
                    CommitMeaning();

                    var sectionName = headingMatch.Groups[1].Value.Trim();
                    if (IsPartOfSpeechSection(sectionName))
                    {
                        currentPartOfSpeech = sectionName;
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
                {
                    continue;
                }

                if (!line.StartsWith("#", StringComparison.Ordinal)
                    || line.StartsWith("#*", StringComparison.Ordinal)
                    || line.StartsWith("#:", StringComparison.Ordinal))
                {
                    continue;
                }

                var definitionText = Regex.Replace(line, @"^#+\s*", string.Empty);
                var cleanDefinition = CleanWikitext(definitionText);

                if (string.IsNullOrWhiteSpace(cleanDefinition))
                {
                    continue;
                }

                currentDefinitions.Add(new DefinitionItem { Definition = cleanDefinition });
            }

            CommitMeaning();
            return meanings;
        }

        private static bool IsPartOfSpeechSection(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                return false;
            }

            return PartOfSpeechKeywords.Any(keyword =>
                sectionName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private static string CleanWikitext(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            text = Regex.Replace(text, @"<!--.*?-->", string.Empty, RegexOptions.Singleline);
            text = Regex.Replace(text, @"<ref[^>]*>.*?</ref>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<[^>]+>", string.Empty);

            text = Regex.Replace(text, @"\[\[([^|\]]*\|)?([^\]]+)\]\]", "$2");
            text = Regex.Replace(text, @"\{\{lien\|([^|}]+)(?:\|[^}]*)?\}\}", "$1", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"\{\{[^}]+\}\}", string.Empty);

            text = text.Replace("'''", string.Empty).Replace("''", string.Empty);
            text = WebUtility.HtmlDecode(text);
            text = Regex.Replace(text, @"\s+", " ").Trim();

            return text;
        }

        private static string NormalizeFrenchWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return word;
            }

            var normalized = word
                .Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();

            return new string(normalized)
                .Normalize(NormalizationForm.FormC)
                .Replace('\u2019', '\'');
        }
    }
}
