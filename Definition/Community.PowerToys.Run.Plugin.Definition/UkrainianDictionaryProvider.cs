using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Community.PowerToys.Run.Plugin.Definition
{
    /// <summary>
    /// Ukrainian dictionary provider using Ukrainian Wiktionary API (primary) with goroh.pp.ua (fallback).
    /// Wiktionary API is fast (~0.3-0.9s) and returns structured JSON.
    /// goroh.pp.ua is slower (~6-15s) but has richer content from the academic dictionary (SUM-11).
    /// </summary>
    internal class UkrainianDictionaryProvider : IDictionaryProvider
    {
        private readonly HttpClient _httpClient;
        public string LanguageCode => "uk";
        public string DisplayName => "Українська (Вікісловник + goroh.pp.ua)";

        private const string WiktionaryApiBase = "https://uk.wiktionary.org/w/api.php";
        private const string GorohBaseUrl = "https://goroh.pp.ua/Тлумачення/";

        public UkrainianDictionaryProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token)
        {
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Starting lookup for: '{word}'");

            try
            {
                var results = await LookupWiktionaryAsync(word, token);
                if (results.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Wiktionary returned {results.Sum(e => e.Meanings.Sum(m => m.Definitions.Count))} definitions");
                    return results;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Wiktionary failed: {ex.GetType().Name}: {ex.Message}");
            }

            try
            {
                var results = await LookupGorohAsync(word, token);
                if (results.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] goroh.pp.ua returned {results.Sum(e => e.Meanings.Sum(m => m.Definitions.Count))} definitions");
                    return results;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] goroh.pp.ua fallback failed: {ex.GetType().Name}: {ex.Message}");
            }

            return new List<DictionaryEntry>();
        }

        #region Wiktionary API (primary)

        private async Task<List<DictionaryEntry>> LookupWiktionaryAsync(string word, CancellationToken token)
        {
            var encodedWord = Uri.EscapeDataString(word);
            var url = $"{WiktionaryApiBase}?action=parse&page={encodedWord}&prop=wikitext&format=json&redirects=1";

            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Wiktionary URL: {url}");

            using var response = await _httpClient.GetAsync(url, token);

            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Wiktionary HTTP {(int)response.StatusCode}");
                return new List<DictionaryEntry>();
            }

            var json = await response.Content.ReadAsStringAsync(token);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("error", out _))
            {
                return new List<DictionaryEntry>();
            }

            if (!root.TryGetProperty("parse", out var parse) ||
                !parse.TryGetProperty("wikitext", out var wikitext) ||
                !wikitext.TryGetProperty("*", out var wikitextContent))
            {
                return new List<DictionaryEntry>();
            }

            var text = wikitextContent.GetString();
            if (string.IsNullOrEmpty(text))
                return new List<DictionaryEntry>();

            return ParseWikitext(word, text);
        }

        private List<DictionaryEntry> ParseWikitext(string word, string wikitext)
        {
            var ukSection = ExtractUkrainianSection(wikitext);
            if (string.IsNullOrEmpty(ukSection))
                return new List<DictionaryEntry>();

            var entry = new DictionaryEntry
            {
                Word = word,
                SourceUrls = new List<string> { $"https://uk.wiktionary.org/wiki/{Uri.EscapeDataString(word)}" }
            };

            var pos = ExtractPartOfSpeech(ukSection);
            var definitions = ExtractDefinitions(ukSection);

            if (definitions.Count == 0)
                return new List<DictionaryEntry>();

            var meaning = new Meaning
            {
                PartOfSpeech = pos,
                Definitions = definitions
            };

            var synonyms = ExtractSynonyms(ukSection);
            if (synonyms.Count > 0)
                meaning.Synonyms = synonyms;

            var antonyms = ExtractAntonyms(ukSection);
            if (antonyms.Count > 0)
                meaning.Antonyms = antonyms;

            entry.Meanings.Add(meaning);
            return new List<DictionaryEntry> { entry };
        }

        private string ExtractUkrainianSection(string wikitext)
        {
            var ukStart = wikitext.IndexOf("{{=uk=}}", StringComparison.Ordinal);
            if (ukStart < 0)
            {
                // Some pages use alternative markers
                ukStart = wikitext.IndexOf("== Українська ==", StringComparison.Ordinal);
                if (ukStart < 0)
                {
                    // If there's no explicit language section, the whole page might be Ukrainian
                    if (wikitext.Contains("Значення") || wikitext.Contains("Семантичні"))
                        return wikitext;
                    return null;
                }
            }

            // Find the end of the Ukrainian section (next language section)
            var nextLangPatterns = new[] { "{{=", "== " };
            var ukEnd = wikitext.Length;

            foreach (var pattern in nextLangPatterns)
            {
                var idx = wikitext.IndexOf(pattern, ukStart + 10, StringComparison.Ordinal);
                if (idx > 0)
                {
                    // Make sure it's actually a new language section, not a subsection
                    var lineStart = wikitext.LastIndexOf('\n', idx);
                    var line = wikitext.Substring(lineStart + 1, idx - lineStart - 1 + pattern.Length + 5);
                    if (Regex.IsMatch(line.Trim(), @"^(\{\{=[a-z]+=\}\}|==\s*\p{Lu})"))
                    {
                        ukEnd = Math.Min(ukEnd, idx);
                    }
                }
            }

            return wikitext.Substring(ukStart, ukEnd - ukStart);
        }

        private string ExtractPartOfSpeech(string section)
        {
            // Check morphosyntactic templates
            if (Regex.IsMatch(section, @"\{\{імен\s+uk")) return "іменник";
            if (Regex.IsMatch(section, @"\{\{-ння\|")) return "іменник";
            if (section.Contains("Іменник")) return "іменник";

            if (Regex.IsMatch(section, @"\{\{дієсл\s+uk")) return "дієслово";
            if (section.Contains("Дієслово")) return "дієслово";

            if (Regex.IsMatch(section, @"\{\{прикм\s+uk")) return "прикметник";
            if (section.Contains("Прикметник")) return "прикметник";

            if (Regex.IsMatch(section, @"\{\{присл\s+uk")) return "прислівник";
            if (section.Contains("Прислівник")) return "прислівник";

            if (section.Contains("Займенник")) return "займенник";
            if (section.Contains("Числівник")) return "числівник";
            if (section.Contains("Частка")) return "частка";
            if (section.Contains("Сполучник")) return "сполучник";
            if (section.Contains("Прийменник")) return "прийменник";
            if (section.Contains("Вигук")) return "вигук";

            return string.Empty;
        }

        private List<DefinitionItem> ExtractDefinitions(string section)
        {
            var definitions = new List<DefinitionItem>();

            // Find the "Значення" section
            var znachStart = section.IndexOf("Значення", StringComparison.Ordinal);
            if (znachStart < 0)
                return definitions;

            // Get content after the header
            var afterHeader = section.Substring(znachStart);
            var nextSection = Regex.Match(afterHeader, @"\n===+[^=]");
            var defSection = nextSection.Success
                ? afterHeader.Substring(0, nextSection.Index)
                : afterHeader;

            // Extract numbered definitions (lines starting with #)
            var lines = defSection.Split('\n');
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (!trimmed.StartsWith("#") || trimmed.StartsWith("#*") || trimmed.StartsWith("#:"))
                    continue;

                // Remove leading # and any sub-numbering
                var defText = Regex.Replace(trimmed, @"^#+\s*", "");
                if (string.IsNullOrWhiteSpace(defText))
                    continue;

                string example = null;

                // Extract example from {{приклад|...}}
                var exampleMatch = Regex.Match(defText, @"\{\{приклад\|([^|}]+)");
                if (exampleMatch.Success)
                {
                    example = exampleMatch.Groups[1].Value.Trim();
                }

                // Clean up wikitext markup
                defText = CleanWikitext(defText);
                if (string.IsNullOrWhiteSpace(defText))
                    continue;

                var defItem = new DefinitionItem { Definition = defText };
                if (!string.IsNullOrEmpty(example))
                {
                    defItem.Example = CleanWikitext(example);
                }

                definitions.Add(defItem);

                if (definitions.Count >= 5)
                    break;
            }

            return definitions;
        }

        private List<string> ExtractSynonyms(string section)
        {
            var synonyms = new List<string>();

            // From {{семантика|синоніми=...|...}}
            foreach (Match m in Regex.Matches(section, @"\{\{семантика\|[^}]*синоніми=([^|}]+)"))
            {
                var syns = m.Groups[1].Value.Split(',').Select(s => CleanWikitext(s).Trim())
                    .Where(s => !string.IsNullOrEmpty(s));
                synonyms.AddRange(syns);
            }

            // From dedicated "Синоніми" section
            var synStart = section.IndexOf("Синоніми", StringComparison.Ordinal);
            if (synStart >= 0)
            {
                var afterHeader = section.Substring(synStart);
                var nextSection = Regex.Match(afterHeader, @"\n===+[^=]");
                var synSection = nextSection.Success
                    ? afterHeader.Substring(0, nextSection.Index)
                    : afterHeader;

                foreach (var line in synSection.Split('\n'))
                {
                    var trimmed = line.Trim();
                    if (trimmed.StartsWith("#"))
                    {
                        var text = Regex.Replace(trimmed, @"^#+\s*", "");
                        var cleaned = CleanWikitext(text).Trim();
                        if (!string.IsNullOrEmpty(cleaned) && cleaned != "—")
                        {
                            synonyms.AddRange(cleaned.Split(',').Select(s => s.Trim())
                                .Where(s => !string.IsNullOrEmpty(s)));
                        }
                    }
                }
            }

            return synonyms.Distinct().Take(5).ToList();
        }

        private List<string> ExtractAntonyms(string section)
        {
            var antonyms = new List<string>();

            foreach (Match m in Regex.Matches(section, @"\{\{семантика\|[^}]*антоніми=([^|}]+)"))
            {
                var ants = m.Groups[1].Value.Split(',').Select(s => CleanWikitext(s).Trim())
                    .Where(s => !string.IsNullOrEmpty(s));
                antonyms.AddRange(ants);
            }

            var antStart = section.IndexOf("Антоніми", StringComparison.Ordinal);
            if (antStart >= 0)
            {
                var afterHeader = section.Substring(antStart);
                var nextSection = Regex.Match(afterHeader, @"\n===+[^=]");
                var antSection = nextSection.Success
                    ? afterHeader.Substring(0, nextSection.Index)
                    : afterHeader;

                foreach (var line in antSection.Split('\n'))
                {
                    var trimmed = line.Trim();
                    if (trimmed.StartsWith("#"))
                    {
                        var text = Regex.Replace(trimmed, @"^#+\s*", "");
                        var cleaned = CleanWikitext(text).Trim();
                        if (!string.IsNullOrEmpty(cleaned) && cleaned != "—")
                        {
                            antonyms.AddRange(cleaned.Split(',').Select(s => s.Trim())
                                .Where(s => !string.IsNullOrEmpty(s)));
                        }
                    }
                }
            }

            return antonyms.Distinct().Take(5).ToList();
        }

        private static string CleanWikitext(string text)
        {
            // Remove templates like {{семантика|...}}, {{приклад|...}}, {{комп.|uk}}, etc.
            text = Regex.Replace(text, @"\{\{семантика\|[^}]*\}\}", "");
            text = Regex.Replace(text, @"\{\{приклад\|[^}]*\}\}", "");
            text = Regex.Replace(text, @"\{\{списки семантичних зв'язків\}\}", "");

            // Convert label templates like {{комп.|uk}} → (комп.)
            text = Regex.Replace(text, @"\{\{(\w+)\.\|uk\}\}", "($1.)");
            text = Regex.Replace(text, @"\{\{позначка\|([^}]+)\}\}", "$1");

            // Convert wiki links: [[display|text]] → text, [[text]] → text
            text = Regex.Replace(text, @"\[\[([^|\]]*\|)?([^\]]+)\]\]", "$2");

            // Remove remaining templates
            text = Regex.Replace(text, @"\{\{[^}]*\}\}", "");

            // Clean up whitespace and punctuation artifacts
            text = Regex.Replace(text, @"\s+", " ");
            text = text.Trim().TrimEnd(')').Trim();

            return text;
        }

        #endregion

        #region goroh.pp.ua (fallback)

        private async Task<List<DictionaryEntry>> LookupGorohAsync(string word, CancellationToken token)
        {
            var requestUrl = $"{GorohBaseUrl}{Uri.EscapeDataString(word.ToLowerInvariant())}";
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] goroh.pp.ua URL: {requestUrl}");

            using var response = await _httpClient.GetAsync(requestUrl, token);
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] goroh.pp.ua status: {response.StatusCode}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new List<DictionaryEntry>();

            if (!response.IsSuccessStatusCode)
                return new List<DictionaryEntry>();

            var html = await response.Content.ReadAsStringAsync(token);

            if (html.Contains("isNotFound: true"))
                return new List<DictionaryEntry>();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return ParseGorohHtml(word, doc, requestUrl);
        }

        private List<DictionaryEntry> ParseGorohHtml(string word, HtmlDocument doc, string sourceUrl)
        {
            var articleBlocks = doc.DocumentNode.SelectNodes("//div[contains(@class, 'article-block')]");
            if (articleBlocks == null || articleBlocks.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("[UkrainianProvider] goroh.pp.ua: no article-block found");
                return new List<DictionaryEntry>();
            }

            var entry = new DictionaryEntry
            {
                Word = word,
                SourceUrls = new List<string> { sourceUrl }
            };

            foreach (var block in articleBlocks)
            {
                var titleNode = block.SelectSingleNode(".//h2[contains(@class, 'page__sub-header')]//span[contains(@class, 'uppercase')]");
                var wordTitle = HtmlEntity.DeEntitize(titleNode?.InnerText?.Trim() ?? word.ToUpper());
                wordTitle = wordTitle.Replace("\u0301", "");

                var remarkNode = block.SelectSingleNode(".//h2[contains(@class, 'page__sub-header')]//span[contains(@class, 'block-remark')]");
                var pos = ParseGorohPartOfSpeech(remarkNode);

                var formulaNodes = block.SelectNodes(".//span[contains(@class, 'interpret-formula')]");
                if (formulaNodes == null || formulaNodes.Count == 0)
                    continue;

                var meaning = new Meaning { PartOfSpeech = pos };

                var interpretDivs = block.SelectNodes(".//div[contains(@class, 'interpret')]");
                if (interpretDivs != null)
                {
                    foreach (var interpretDiv in interpretDivs.Take(5))
                    {
                        var formulaNode = interpretDiv.SelectSingleNode(".//span[contains(@class, 'interpret-formula')]");
                        if (formulaNode == null) continue;

                        var defText = HtmlEntity.DeEntitize(formulaNode.InnerText?.Trim() ?? "");
                        if (string.IsNullOrEmpty(defText)) continue;

                        var displayText = defText.Length > 250 ? defText.Substring(0, 247) + "..." : defText;
                        var defItem = new DefinitionItem { Definition = displayText };

                        var exampleNode = interpretDiv.SelectSingleNode(".//span[contains(@class, 'example-text')]");
                        if (exampleNode != null)
                        {
                            var exampleText = HtmlEntity.DeEntitize(exampleNode.InnerText?.Trim() ?? "");
                            if (!string.IsNullOrEmpty(exampleText))
                            {
                                var sourceNode = exampleNode.SelectSingleNode("following-sibling::span[contains(@class, 'example-source')]")
                                    ?? interpretDiv.SelectSingleNode(".//span[contains(@class, 'example-source')]");
                                var source = sourceNode != null ? " " + HtmlEntity.DeEntitize(sourceNode.InnerText?.Trim() ?? "") : "";
                                defItem.Example = exampleText.Length > 200
                                    ? exampleText.Substring(0, 197) + "..."
                                    : exampleText + source;
                            }
                        }

                        meaning.Definitions.Add(defItem);
                    }
                }
                else
                {
                    foreach (var formulaNode in formulaNodes.Take(5))
                    {
                        var defText = HtmlEntity.DeEntitize(formulaNode.InnerText?.Trim() ?? "");
                        if (!string.IsNullOrEmpty(defText))
                        {
                            var displayText = defText.Length > 250 ? defText.Substring(0, 247) + "..." : defText;
                            meaning.Definitions.Add(new DefinitionItem { Definition = displayText });
                        }
                    }
                }

                if (meaning.Definitions.Count > 0)
                    entry.Meanings.Add(meaning);
            }

            if (entry.Meanings.Count == 0)
                return new List<DictionaryEntry>();

            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] goroh.pp.ua: found {entry.Meanings.Sum(m => m.Definitions.Count)} definitions");
            return new List<DictionaryEntry> { entry };
        }

        private string ParseGorohPartOfSpeech(HtmlNode remarkNode)
        {
            if (remarkNode == null) return string.Empty;

            var text = HtmlEntity.DeEntitize(remarkNode.InnerText?.Trim() ?? "").ToLowerInvariant();

            var genderNode = remarkNode.SelectSingleNode(".//span[@title]");
            if (genderNode != null)
            {
                var title = genderNode.GetAttributeValue("title", "").ToLowerInvariant();
                if (title.Contains("жіночий")) return "іменник (ж.)";
                if (title.Contains("чоловічий")) return "іменник (ч.)";
                if (title.Contains("середній")) return "іменник (с.)";
            }

            if (text.Contains("ж.")) return "іменник (ж.)";
            if (text.Contains("ч.")) return "іменник (ч.)";
            if (text.Contains("с.")) return "іменник (с.)";

            return string.Empty;
        }

        #endregion
    }
}
