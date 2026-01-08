using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Community.PowerToys.Run.Plugin.Definition
{
    /// <summary>
    /// Chinese dictionary provider using MDBG.net (CC-CEDICT data)
    /// Supports both Chinese characters and Pinyin queries
    /// </summary>
    internal class ChineseDictionaryProvider : IDictionaryProvider
    {
        private readonly HttpClient _httpClient;
        public string LanguageCode => "zh";
        public string DisplayName => "中文 (MDBG.net)";

        private const string DefaultBaseUrl = "https://www.mdbg.net/chinese/dictionary?page=worddict&wdrst=0&wdqb=";

        public ChineseDictionaryProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token)
        {
            System.Diagnostics.Debug.WriteLine($"[ChineseProvider] Starting lookup for word: '{word}'");

            var baseUrl = ConfigurationManager.Configuration.ChineseApiEndpoint;
            
            // Use default if empty
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = DefaultBaseUrl;
            }

            var requestUrl = $"{baseUrl}{Uri.EscapeDataString(word)}";
            System.Diagnostics.Debug.WriteLine($"[ChineseProvider] Request URL: {requestUrl}");

            try
            {
                using var response = await _httpClient.GetAsync(requestUrl, token);
                System.Diagnostics.Debug.WriteLine($"[ChineseProvider] Response status: {response.StatusCode}");

                var html = await response.Content.ReadAsStringAsync(token);
                System.Diagnostics.Debug.WriteLine($"[ChineseProvider] Response length: {html.Length} bytes");
                
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Parse the results table
                var results = ParseSearchResults(word, doc, requestUrl);
                
                if (!results.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"[ChineseProvider] No results found");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[ChineseProvider] Found {results.Count} results");
                }

                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ChineseProvider] Error: {ex.Message}");
                throw;
            }
        }

        private List<DictionaryEntry> ParseSearchResults(string query, HtmlDocument doc, string sourceUrl)
        {
            var entries = new List<DictionaryEntry>();

            // Find all result rows in the results table
            // MDBG search results are in table rows with class "row"
            var resultRows = doc.DocumentNode.SelectNodes("//table[contains(@class, 'wordresults')]//tr[contains(@class, 'row')]")
                             ?? doc.DocumentNode.SelectNodes("//tr[contains(@class, 'row')]");
            
            if (resultRows == null || !resultRows.Any())
            {
                System.Diagnostics.Debug.WriteLine("[ChineseProvider] No result rows found");
                return entries;
            }

            // Limit to top 5 results to avoid overwhelming the user
            foreach (var row in resultRows.Take(5))
            {
                try
                {
                    var entry = ParseResultRow(row, sourceUrl);
                    if (entry != null)
                    {
                        entries.Add(entry);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ChineseProvider] Error parsing row: {ex.Message}");
                }
            }

            return entries;
        }

        private DictionaryEntry ParseResultRow(HtmlNode row, string sourceUrl)
        {
            // Extract Chinese characters (simplified and/or traditional)
            var chineseNode = row.SelectSingleNode(".//div[contains(@class, 'hanzi')]");
            if (chineseNode == null) return null;

            var chineseText = GetCleanText(chineseNode);
            if (string.IsNullOrEmpty(chineseText)) return null;

            // Extract Pinyin
            var pinyinNode = row.SelectSingleNode(".//div[contains(@class, 'pinyin')]");
            var pinyin = pinyinNode != null ? GetCleanText(pinyinNode) : string.Empty;

            // Extract English definition
            var defNode = row.SelectSingleNode(".//div[contains(@class, 'defs')]");
            var definition = defNode != null ? GetCleanText(defNode) : string.Empty;

            // Create dictionary entry
            var entry = new DictionaryEntry
            {
                Word = chineseText,
                Phonetic = pinyin,
                SourceUrls = new List<string> { sourceUrl }
            };

            // Add phonetic if available
            if (!string.IsNullOrEmpty(pinyin))
            {
                entry.Phonetics.Add(new Phonetic
                {
                    Text = pinyin
                });
            }

            // Parse definitions (they may be numbered)
            var definitions = ParseDefinitions(definition);
            
            var meaning = new Meaning
            {
                PartOfSpeech = string.Empty, // MDBG doesn't always provide part of speech
                Definitions = definitions
            };

            entry.Meanings.Add(meaning);
            return entry;
        }

        private List<DefinitionItem> ParseDefinitions(string definitionText)
        {
            var definitions = new List<DefinitionItem>();

            if (string.IsNullOrEmpty(definitionText))
            {
                return definitions;
            }

            // Check if definitions are numbered (e.g., "1. first def / 2. second def")
            // MDBG uses " / " to separate multiple definitions
            var parts = definitionText.Split(new[] { " / " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts.Take(3)) // Limit to 3 definitions per entry
            {
                var cleanDef = part.Trim();
                
                // Remove leading numbers if present (e.g., "1. definition" -> "definition")
                cleanDef = Regex.Replace(cleanDef, @"^\d+\.\s*", "");
                
                if (!string.IsNullOrEmpty(cleanDef))
                {
                    definitions.Add(new DefinitionItem { Definition = cleanDef });
                }
            }

            // If no parts were found, add the whole text as one definition
            if (!definitions.Any() && !string.IsNullOrEmpty(definitionText))
            {
                definitions.Add(new DefinitionItem { Definition = definitionText.Trim() });
            }

            return definitions;
        }

        private string GetCleanText(HtmlNode node)
        {
            if (node == null) return string.Empty;
            
            var text = node.InnerText;
            
            // Decode HTML entities
            text = System.Net.WebUtility.HtmlDecode(text);
            
            // Replace multiple whitespace with single space
            text = Regex.Replace(text, @"\s+", " ");
            
            return text.Trim();
        }
    }
}
