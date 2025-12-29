using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class UkrainianDictionaryProvider : IDictionaryProvider
    {
        private readonly HttpClient _httpClient;
        public string LanguageCode => "uk";
        public string DisplayName => "Українська (sum.in.ua)";

        public UkrainianDictionaryProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token)
        {
            var baseUrl = ConfigurationManager.Configuration.UkrainianApiEndpoint;
            if (string.IsNullOrEmpty(baseUrl)) baseUrl = "https://sum.in.ua/s/";
            
            // sum.in.ua uses Latin transliteration in URLs, not Cyrillic
            var transliteratedWord = TransliterateCyrillicToLatin(word);
            var requestUrl = $"{baseUrl}{Uri.EscapeDataString(transliteratedWord)}";

            using var response = await _httpClient.GetAsync(requestUrl, token);

            // Note: sum.in.ua returns 404 for ALL requests, even for existing words
            // The actual "not found" status is indicated in the HTML content with "не знайдено"
            // So we should not early-return on 404, but instead parse the HTML to check

            var stream = await response.Content.ReadAsStreamAsync(token);
            var doc = new HtmlDocument();
            // Force UTF-8 encoding to avoid execution environment specific defaults (e.g. Windows-1252)
            doc.Load(stream, System.Text.Encoding.UTF8);

            // sum.in.ua structure: definitions are primarily in div with itemprop="articleBody"
            // We prioritize this as it's cleaner and avoids nested div#article issues
            var articleNodes = doc.DocumentNode.SelectNodes("//div[@itemprop='articleBody']");
            
            if (articleNodes == null || articleNodes.Count == 0)
            {
                // Fallback to div#article if itemprop not found
                articleNodes = doc.DocumentNode.SelectNodes("//div[@id='article']");
            }

            if (articleNodes == null || articleNodes.Count == 0)
            {
                // Check for "word not found" page with suggestions
                // Since we read as stream, accessing InnerText or OuterHtml of DocumentNode
                var pageContent = doc.DocumentNode.OuterHtml;
                if (pageContent.Contains("не знайдено") || pageContent.Contains("Можливо, ви шукали"))
                {
                    return new List<DictionaryEntry>();
                }
            }

            if (articleNodes == null) return new List<DictionaryEntry>();

            var entries = new List<DictionaryEntry>();
            foreach (var node in articleNodes)
            {
                var entry = new DictionaryEntry
                {
                    Word = word,
                    SourceUrls = new List<string> { requestUrl }
                };

                // Extracting text and cleaning it up
                // sum.in.ua often uses <em> for grammar, <b> for word
                var cleanText = node.InnerText.Trim();
                
                // Simple parsing for sum.in.ua:
                // Usually starts with WORD, grammar info, then definition.
                // We'll put the whole text as one definition for now, 
                // but ideally we split by numbered meanings if they exist (1., 2., ...)
                
                var meaning = new Meaning
                {
                    PartOfSpeech = ExtractPartOfSpeech(cleanText),
                    Definitions = new List<DefinitionItem> 
                    { 
                        new DefinitionItem { Definition = cleanText } 
                    }
                };
                
                entry.Meanings.Add(meaning);
                entries.Add(entry);
            }

            return entries;
        }

        private string TransliterateCyrillicToLatin(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Transliteration map based on sum.in.ua URL scheme
            var transliteration = new Dictionary<char, string>
            {
                {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'ґ', "g"}, {'д', "d"},
                {'е', "e"}, {'є', "je"}, {'ж', "zh"}, {'з', "z"}, {'и', "y"}, {'і', "i"},
                {'ї', "ji"}, {'й', "j"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
                {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"}, {'у', "u"},
                {'ф', "f"}, {'х', "h"}, {'ц', "c"}, {'ч', "ch"}, {'ш', "sh"}, {'щ', "shh"},
                {'ь', "j"}, {'ю', "ju"}, {'я', "ja"},
                {'А', "A"}, {'Б', "B"}, {'В', "V"}, {'Г', "G"}, {'Ґ', "G"}, {'Д', "D"},
                {'Е', "E"}, {'Є', "Je"}, {'Ж', "Zh"}, {'З', "Z"}, {'И', "Y"}, {'І', "I"},
                {'Ї', "Ji"}, {'Й', "J"}, {'К', "K"}, {'Л', "L"}, {'М', "M"}, {'Н', "N"},
                {'О', "O"}, {'П', "P"}, {'Р', "R"}, {'С', "S"}, {'Т', "T"}, {'У', "U"},
                {'Ф', "F"}, {'Х', "H"}, {'Ц', "C"}, {'Ч', "Ch"}, {'Ш', "Sh"}, {'Щ', "Shh"},
                {'Ь', "J"}, {'Ю', "Ju"}, {'Я', "Ja"}
            };

            var result = new System.Text.StringBuilder(input.Length * 2);
            foreach (var c in input)
            {
                if (transliteration.TryGetValue(c, out string replacement))
                    result.Append(replacement);
                else
                    result.Append(c);
            }

            return result.ToString();
        }

        private string ExtractPartOfSpeech(string text)
        {
            if (text.Contains("іменник")) return "noun";
            if (text.Contains("дієслово")) return "verb";
            if (text.Contains("прикметник")) return "adjective";
            if (text.Contains("прислівник")) return "adverb";
            if (text.Contains("чол.") || text.Contains("жін.") || text.Contains("сер.")) return "noun";
            return "unknown";
        }
    }
}
