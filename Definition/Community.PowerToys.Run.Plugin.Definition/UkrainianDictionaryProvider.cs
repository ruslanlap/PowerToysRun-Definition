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
    /// Ukrainian dictionary provider using sum.in.ua
    /// Uses transliterated URL paths: https://sum.in.ua/s/{word}
    /// </summary>
    internal class UkrainianDictionaryProvider : IDictionaryProvider
    {
        private readonly HttpClient _httpClient;
        public string LanguageCode => "uk";
        public string DisplayName => "Українська (sum.in.ua)";

        private const string DefaultBaseUrl = "https://sum.in.ua/s/";

        public UkrainianDictionaryProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token)
        {
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Starting lookup for word: '{word}'");

            // Transliterate Cyrillic to Latin for URL
            var transliteratedWord = TransliterateCyrillicToLatin(word.ToLowerInvariant());
            var baseUrl = ConfigurationManager.Configuration.UkrainianApiEndpoint;
            
            // Use default if empty or if using old query parameter approach
            if (string.IsNullOrEmpty(baseUrl) || baseUrl.Contains("?swrd="))
            {
                baseUrl = DefaultBaseUrl;
            }

            var requestUrl = $"{baseUrl}{transliteratedWord}";
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Transliterated '{word}' -> '{transliteratedWord}'");
            System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Request URL: {requestUrl}");

            try
            {
                using var response = await _httpClient.GetAsync(requestUrl, token);
                System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Response status: {response.StatusCode}");

                // Note: sum.in.ua may return 404 for existing words, so we don't check status code
                var html = await response.Content.ReadAsStringAsync(token);
                System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Response length: {html.Length} bytes");
                
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Try to get the article body (definition content)
                var articleBody = doc.DocumentNode.SelectSingleNode("//div[@itemprop='articleBody']");
                
                if (articleBody != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Found articleBody");
                    return ParseArticleBody(word, articleBody, requestUrl);
                }

                // Fallback: Try the #article selector
                var articleNode = doc.DocumentNode.SelectSingleNode("//div[@id='article']//div[@itemprop='articleBody']")
                                ?? doc.DocumentNode.SelectSingleNode("//div[@id='textside']//div[@itemprop='articleBody']");
                if (articleNode != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Found article via fallback selector");
                    return ParseArticleBody(word, articleNode, requestUrl);
                }

                // Check if it's a "not found" page
                var pageContent = doc.DocumentNode.InnerText;
                if (pageContent.Contains("не знайдено") || pageContent.Contains("Можливо, ви шукали"))
                {
                    System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Word not found");
                    return new List<DictionaryEntry>();
                }

                // Check for search alternatives
                var searchRes = doc.DocumentNode.SelectSingleNode("//div[@id='search-res']");
                if (searchRes != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Found alternatives");
                    return ParseAlternatives(word, searchRes, requestUrl);
                }

                System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] No content found");
                return new List<DictionaryEntry>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UkrainianProvider] Error: {ex.Message}");
                throw;
            }
        }

        private List<DictionaryEntry> ParseArticleBody(string word, HtmlNode articleBody, string sourceUrl)
        {
            var entry = new DictionaryEntry
            {
                Word = word,
                SourceUrls = new List<string> { sourceUrl }
            };

            // Extract the word title
            var titleNode = articleBody.SelectSingleNode(".//strong[@class='title']");
            var wordTitle = titleNode?.InnerText?.Trim() ?? word.ToUpper();

            // Get all text content
            var fullText = GetCleanText(articleBody);
            
            // Extract part of speech
            var pos = ExtractPartOfSpeech(articleBody, fullText);

            // Split into numbered definitions
            var definitions = SplitDefinitions(articleBody, fullText);

            var meaning = new Meaning
            {
                PartOfSpeech = pos,
                Definitions = definitions
            };

            entry.Meanings.Add(meaning);
            return new List<DictionaryEntry> { entry };
        }

        private List<DictionaryEntry> ParseAlternatives(string word, HtmlNode searchRes, string sourceUrl)
        {
            var entry = new DictionaryEntry
            {
                Word = word,
                SourceUrls = new List<string> { sourceUrl }
            };

            var headerNode = searchRes.SelectSingleNode(".//p");
            var headerText = headerNode?.InnerText?.Trim() ?? "Слово не знайдено";

            var alternatives = new List<string>();
            var listItems = searchRes.SelectNodes(".//li");
            if (listItems != null)
            {
                foreach (var li in listItems.Take(5))
                {
                    var altWord = li.InnerText?.Trim();
                    if (!string.IsNullOrEmpty(altWord))
                    {
                        alternatives.Add(altWord);
                    }
                }
            }

            var definitionText = alternatives.Any()
                ? $"{headerText}: {string.Join(", ", alternatives)}"
                : headerText;

            var meaning = new Meaning
            {
                PartOfSpeech = string.Empty,
                Definitions = new List<DefinitionItem>
                {
                    new DefinitionItem { Definition = definitionText }
                }
            };

            entry.Meanings.Add(meaning);
            return new List<DictionaryEntry> { entry };
        }

        private string GetCleanText(HtmlNode node)
        {
            var text = node.InnerText;
            text = Regex.Replace(text, @"\s+", " ");
            return text.Trim();
        }

        private List<DefinitionItem> SplitDefinitions(HtmlNode articleBody, string fullText)
        {
            var definitions = new List<DefinitionItem>();

            // Try to find numbered definitions by looking for span.zn elements
            var znNodes = articleBody.SelectNodes(".//span[@class='zn']");
            
            if (znNodes != null && znNodes.Count > 1)
            {
                // Multiple numbered definitions
                var pNodes = articleBody.SelectNodes(".//p[@class='znach']");
                if (pNodes != null)
                {
                    foreach (var p in pNodes.Take(5)) // Limit to 5 definitions
                    {
                        var defText = GetCleanText(p);
                        // Remove leading number
                        defText = Regex.Replace(defText, @"^\d+\.\s*", "");
                        
                        if (!string.IsNullOrEmpty(defText))
                        {
                            // Truncate for display
                            var displayText = defText.Length > 250 
                                ? defText.Substring(0, 247) + "..." 
                                : defText;
                            definitions.Add(new DefinitionItem { Definition = displayText });
                        }
                    }
                }
            }

            // If no numbered definitions found, use full text
            if (!definitions.Any())
            {
                // Remove the word title from the beginning
                var text = fullText;
                var titleMatch = Regex.Match(text, @"^[А-ЯІЇЄҐ]+,?\s*");
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

        private string ExtractPartOfSpeech(HtmlNode articleBody, string fullText)
        {
            // Look for abbreviation tags
            var abbrNode = articleBody.SelectSingleNode(".//abbr[@class='mark']");
            if (abbrNode != null)
            {
                var title = abbrNode.GetAttributeValue("title", "");
                var text = abbrNode.InnerText?.Trim() ?? "";

                if (title.Contains("чоловічий") || text == "чол.") return "noun (m)";
                if (title.Contains("жіночий") || text == "жін.") return "noun (f)";
                if (title.Contains("середній") || text == "сер.") return "noun (n)";
            }

            // Fallback to text patterns
            if (fullText.Contains("іменник")) return "noun";
            if (fullText.Contains("дієслово")) return "verb";
            if (fullText.Contains("прикметник")) return "adjective";
            if (fullText.Contains("прислівник")) return "adverb";
            if (fullText.Contains("займенник")) return "pronoun";
            if (fullText.Contains("числівник")) return "numeral";
            if (fullText.Contains("частка")) return "particle";
            if (fullText.Contains("сполучник")) return "conjunction";
            if (fullText.Contains("прийменник")) return "preposition";
            if (fullText.Contains("вигук")) return "interjection";
            
            return string.Empty;
        }

        /// <summary>
        /// Transliterates Ukrainian Cyrillic to Latin for sum.in.ua URL paths.
        /// Based on the scheme used by sum.in.ua website.
        /// </summary>
        private string TransliterateCyrillicToLatin(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var transliteration = new Dictionary<char, string>
            {
                // Lowercase
                {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "gh"}, {'ґ', "g"}, {'д', "d"},
                {'е', "e"}, {'є', "je"}, {'ж', "zh"}, {'з', "z"}, {'и', "y"}, {'і', "i"},
                {'ї', "ji"}, {'й', "j"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
                {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"}, {'у', "u"},
                {'ф', "f"}, {'х', "kh"}, {'ц', "c"}, {'ч', "ch"}, {'ш', "sh"}, {'щ', "shh"},
                {'ь', "j"}, {'ю', "ju"}, {'я', "ja"}, {'\'', "."}, {'\u2019', "."},
                // Uppercase (converted to lowercase in input, but keep for safety)
                {'А', "a"}, {'Б', "b"}, {'В', "v"}, {'Г', "gh"}, {'Ґ', "g"}, {'Д', "d"},
                {'Е', "e"}, {'Є', "je"}, {'Ж', "zh"}, {'З', "z"}, {'И', "y"}, {'І', "i"},
                {'Ї', "ji"}, {'Й', "j"}, {'К', "k"}, {'Л', "l"}, {'М', "m"}, {'Н', "n"},
                {'О', "o"}, {'П', "p"}, {'Р', "r"}, {'С', "s"}, {'Т', "t"}, {'У', "u"},
                {'Ф', "f"}, {'Х', "kh"}, {'Ц', "c"}, {'Ч', "ch"}, {'Ш', "sh"}, {'Щ', "shh"},
                {'Ь', "j"}, {'Ю', "ju"}, {'Я', "ja"}
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
    }
}
