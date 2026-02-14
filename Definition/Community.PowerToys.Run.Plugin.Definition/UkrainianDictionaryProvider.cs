using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Community.PowerToys.Run.Plugin.Definition
{
    /// <summary>
    /// Ukrainian dictionary provider using goroh.pp.ua (primary) with sum.in.ua (fallback).
    /// goroh.pp.ua uses Cyrillic in URL paths: https://goroh.pp.ua/Тлумачення/{word}
    /// sum.in.ua uses transliterated URL paths: https://sum.in.ua/s/{word}
    /// </summary>
    internal class UkrainianDictionaryProvider : IDictionaryProvider
    {
        private readonly HttpClient _httpClient;
        public string LanguageCode => "uk";
        public string DisplayName => "Українська (goroh.pp.ua)";

        private const string GorohBaseUrl = "https://goroh.pp.ua/Тлумачення/";
        private const string SumBaseUrl = "https://sum.in.ua/s/";

        public UkrainianDictionaryProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token)
        {
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Starting lookup for word: '{word}'");

            // Try goroh.pp.ua first (primary source)
            try
            {
                var results = await LookupGorohAsync(word, token);
                if (results.Count > 0)
                {
                    return results;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] goroh.pp.ua failed: {ex.Message}");
            }

            // Fallback to sum.in.ua
            try
            {
                var results = await LookupSumAsync(word, token);
                if (results.Count > 0)
                {
                    return results;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] sum.in.ua fallback failed: {ex.Message}");
            }

            return new List<DictionaryEntry>();
        }

        #region goroh.pp.ua (primary)

        private async Task<List<DictionaryEntry>> LookupGorohAsync(string word, CancellationToken token)
        {
            // goroh.pp.ua accepts Cyrillic directly in URL
            var requestUrl = $"{GorohBaseUrl}{Uri.EscapeDataString(word.ToLowerInvariant())}";
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] goroh.pp.ua URL: {requestUrl}");

            using var response = await _httpClient.GetAsync(requestUrl, token);
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] goroh.pp.ua status: {response.StatusCode}");

            // 404 means word not found
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<DictionaryEntry>();
            }

            if (!response.IsSuccessStatusCode)
            {
                return new List<DictionaryEntry>();
            }

            var html = await response.Content.ReadAsStringAsync(token);

            // Check for isNotFound in page data
            if (html.Contains("isNotFound: true"))
            {
                return new List<DictionaryEntry>();
            }

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
                // Extract word title from h2.page__sub-header > span.uppercase
                var titleNode = block.SelectSingleNode(".//h2[contains(@class, 'page__sub-header')]//span[contains(@class, 'uppercase')]");
                var wordTitle = HtmlEntity.DeEntitize(titleNode?.InnerText?.Trim() ?? word.ToUpper());
                // Remove stress marks for cleaner display
                wordTitle = wordTitle.Replace("\u0301", "");

                // Extract part of speech from span.block-remark
                var remarkNode = block.SelectSingleNode(".//h2[contains(@class, 'page__sub-header')]//span[contains(@class, 'block-remark')]");
                var pos = ParsePartOfSpeech(remarkNode);

                // Extract definitions from span.interpret-formula
                var formulaNodes = block.SelectNodes(".//span[contains(@class, 'interpret-formula')]");
                if (formulaNodes == null || formulaNodes.Count == 0)
                {
                    continue;
                }

                var meaning = new Meaning
                {
                    PartOfSpeech = pos
                };

                // Get interpret divs which contain both formula and examples
                var interpretDivs = block.SelectNodes(".//div[contains(@class, 'interpret')]");

                if (interpretDivs != null)
                {
                    foreach (var interpretDiv in interpretDivs.Take(5))
                    {
                        var formulaNode = interpretDiv.SelectSingleNode(".//span[contains(@class, 'interpret-formula')]");
                        if (formulaNode == null)
                            continue;

                        var defText = HtmlEntity.DeEntitize(formulaNode.InnerText?.Trim() ?? "");
                        if (string.IsNullOrEmpty(defText))
                            continue;

                        var displayText = defText.Length > 250
                            ? defText.Substring(0, 247) + "..."
                            : defText;

                        var defItem = new DefinitionItem { Definition = displayText };

                        // Extract first example if available
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
                    // Fallback: just get formulas without examples
                    foreach (var formulaNode in formulaNodes.Take(5))
                    {
                        var defText = HtmlEntity.DeEntitize(formulaNode.InnerText?.Trim() ?? "");
                        if (!string.IsNullOrEmpty(defText))
                        {
                            var displayText = defText.Length > 250
                                ? defText.Substring(0, 247) + "..."
                                : defText;
                            meaning.Definitions.Add(new DefinitionItem { Definition = displayText });
                        }
                    }
                }

                if (meaning.Definitions.Count > 0)
                {
                    entry.Meanings.Add(meaning);
                }
            }

            if (entry.Meanings.Count == 0)
            {
                return new List<DictionaryEntry>();
            }

            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] goroh.pp.ua: found {entry.Meanings.Sum(m => m.Definitions.Count)} definitions");
            return new List<DictionaryEntry> { entry };
        }

        private string ParsePartOfSpeech(HtmlNode remarkNode)
        {
            if (remarkNode == null)
                return string.Empty;

            var text = HtmlEntity.DeEntitize(remarkNode.InnerText?.Trim() ?? "").ToLowerInvariant();

            // Check for gender indicators from goroh.pp.ua format like "и, ж." or "а, ч."
            var genderNode = remarkNode.SelectSingleNode(".//span[@title]");
            if (genderNode != null)
            {
                var title = genderNode.GetAttributeValue("title", "").ToLowerInvariant();
                if (title.Contains("жіночий")) return "іменник (ж.)";
                if (title.Contains("чоловічий")) return "іменник (ч.)";
                if (title.Contains("середній")) return "іменник (с.)";
            }

            // Fallback: check text patterns
            if (text.Contains("ж.")) return "іменник (ж.)";
            if (text.Contains("ч.")) return "іменник (ч.)";
            if (text.Contains("с.")) return "іменник (с.)";

            return string.Empty;
        }

        #endregion

        #region sum.in.ua (fallback)

        private async Task<List<DictionaryEntry>> LookupSumAsync(string word, CancellationToken token)
        {
            var transliteratedWord = TransliterateCyrillicToLatin(word.ToLowerInvariant());

            var baseUrl = ConfigurationManager.Configuration.UkrainianApiEndpoint;
            if (string.IsNullOrEmpty(baseUrl) || !baseUrl.Contains("sum.in.ua"))
            {
                baseUrl = SumBaseUrl;
            }

            var requestUrl = $"{baseUrl}{transliteratedWord}";
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] sum.in.ua URL: {requestUrl}");

            using var response = await _httpClient.GetAsync(requestUrl, token);
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] sum.in.ua status: {response.StatusCode}");

            // sum.in.ua may return 404 for existing words, so we parse HTML regardless
            var html = await response.Content.ReadAsStringAsync(token);
            if (string.IsNullOrEmpty(html))
            {
                return new List<DictionaryEntry>();
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Try to get the article body
            var articleBody = doc.DocumentNode.SelectSingleNode("//div[@itemprop='articleBody']")
                ?? doc.DocumentNode.SelectSingleNode("//div[@id='article']//div[@itemprop='articleBody']")
                ?? doc.DocumentNode.SelectSingleNode("//div[@id='textside']//div[@itemprop='articleBody']");

            if (articleBody != null)
            {
                return ParseSumArticleBody(word, articleBody, requestUrl);
            }

            // Check for "not found" page
            var pageContent = doc.DocumentNode.InnerText;
            if (pageContent.Contains("не знайдено") || pageContent.Contains("Можливо, ви шукали"))
            {
                return new List<DictionaryEntry>();
            }

            return new List<DictionaryEntry>();
        }

        private List<DictionaryEntry> ParseSumArticleBody(string word, HtmlNode articleBody, string sourceUrl)
        {
            var entry = new DictionaryEntry
            {
                Word = word,
                SourceUrls = new List<string> { sourceUrl }
            };

            var fullText = GetCleanText(articleBody);
            var pos = ExtractSumPartOfSpeech(articleBody, fullText);
            var definitions = SplitSumDefinitions(articleBody, fullText);

            var meaning = new Meaning
            {
                PartOfSpeech = pos,
                Definitions = definitions
            };

            entry.Meanings.Add(meaning);
            return new List<DictionaryEntry> { entry };
        }

        private string GetCleanText(HtmlNode node)
        {
            var text = HtmlEntity.DeEntitize(node.InnerText ?? "");
            text = Regex.Replace(text, @"\s+", " ");
            return text.Trim();
        }

        private List<DefinitionItem> SplitSumDefinitions(HtmlNode articleBody, string fullText)
        {
            var definitions = new List<DefinitionItem>();

            var znNodes = articleBody.SelectNodes(".//span[@class='zn']");
            if (znNodes != null && znNodes.Count > 1)
            {
                var pNodes = articleBody.SelectNodes(".//p[@class='znach']");
                if (pNodes != null)
                {
                    foreach (var p in pNodes.Take(5))
                    {
                        var defText = GetCleanText(p);
                        defText = Regex.Replace(defText, @"^\d+\.\s*", "");

                        if (!string.IsNullOrEmpty(defText))
                        {
                            var displayText = defText.Length > 250
                                ? defText.Substring(0, 247) + "..."
                                : defText;
                            definitions.Add(new DefinitionItem { Definition = displayText });
                        }
                    }
                }
            }

            if (!definitions.Any())
            {
                var text = fullText;
                var titleMatch = Regex.Match(text, @"^[А-ЯІЇЄҐ\u0301]+,?\s*");
                if (titleMatch.Success)
                {
                    text = text.Substring(titleMatch.Length);
                }

                var displayText = text.Length > 400
                    ? text.Substring(0, 397) + "..."
                    : text;
                definitions.Add(new DefinitionItem { Definition = displayText.Trim() });
            }

            return definitions;
        }

        private string ExtractSumPartOfSpeech(HtmlNode articleBody, string fullText)
        {
            var abbrNode = articleBody.SelectSingleNode(".//abbr[@class='mark']");
            if (abbrNode != null)
            {
                var title = abbrNode.GetAttributeValue("title", "");
                var text = abbrNode.InnerText?.Trim() ?? "";

                if (title.Contains("чоловічий") || text == "чол.") return "іменник (ч.)";
                if (title.Contains("жіночий") || text == "жін.") return "іменник (ж.)";
                if (title.Contains("середній") || text == "сер.") return "іменник (с.)";
            }

            if (fullText.Contains("іменник")) return "іменник";
            if (fullText.Contains("дієслово")) return "дієслово";
            if (fullText.Contains("прикметник")) return "прикметник";
            if (fullText.Contains("прислівник")) return "прислівник";
            if (fullText.Contains("займенник")) return "займенник";
            if (fullText.Contains("числівник")) return "числівник";
            if (fullText.Contains("частка")) return "частка";
            if (fullText.Contains("сполучник")) return "сполучник";
            if (fullText.Contains("прийменник")) return "прийменник";
            if (fullText.Contains("вигук")) return "вигук";

            return string.Empty;
        }

        #endregion

        #region Transliteration (for sum.in.ua)

        /// <summary>
        /// Transliterates Ukrainian Cyrillic to Latin for sum.in.ua URL paths.
        /// </summary>
        private string TransliterateCyrillicToLatin(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var transliteration = new Dictionary<char, string>
            {
                {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "gh"}, {'ґ', "g"}, {'д', "d"},
                {'е', "e"}, {'є', "je"}, {'ж', "zh"}, {'з', "z"}, {'и', "y"}, {'і', "i"},
                {'ї', "ji"}, {'й', "j"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
                {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"}, {'у', "u"},
                {'ф', "f"}, {'х', "kh"}, {'ц', "c"}, {'ч', "ch"}, {'ш', "sh"}, {'щ', "shh"},
                {'ь', "j"}, {'ю', "ju"}, {'я', "ja"}, {'\'', "."}, {'\u2019', "."}
            };

            var result = new StringBuilder(input.Length * 2);
            foreach (var c in input)
            {
                if (transliteration.TryGetValue(c, out string replacement))
                    result.Append(replacement);
                else
                    result.Append(c);
            }

            return result.ToString();
        }

        #endregion
    }
}
