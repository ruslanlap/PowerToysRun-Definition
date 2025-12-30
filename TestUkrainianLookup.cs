using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

class Program
{
    static async Task Main(string[] args)
    {
        var word = args.Length > 0 ? args[0] : "слово";
        Console.WriteLine($"Testing lookup for: {word}");
        
        var transliterated = TransliterateCyrillicToLatin(word.ToLowerInvariant());
        Console.WriteLine($"Transliterated: {transliterated}");
        
        var url = $"https://sum.in.ua/s/{transliterated}";
        Console.WriteLine($"URL: {url}");
        
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        
        try
        {
            var response = await client.GetAsync(url);
            Console.WriteLine($"Status: {response.StatusCode}");
            
            var html = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response length: {html.Length} bytes");
            
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            
            var articleBody = doc.DocumentNode.SelectSingleNode("//div[@itemprop='articleBody']");
            
            if (articleBody != null)
            {
                Console.WriteLine("\n✅ Found articleBody!");
                var text = articleBody.InnerText.Trim();
                Console.WriteLine($"Content (first 300 chars):\n{text.Substring(0, Math.Min(300, text.Length))}...");
            }
            else
            {
                Console.WriteLine("\n❌ articleBody NOT found");
                
                // Check what we have
                var article = doc.DocumentNode.SelectSingleNode("//div[@id='article']");
                Console.WriteLine($"div#article found: {article != null}");
                
                var textside = doc.DocumentNode.SelectSingleNode("//div[@id='textside']");
                Console.WriteLine($"div#textside found: {textside != null}");
                
                // Check for "not found"
                if (html.Contains("не знайдено"))
                {
                    Console.WriteLine("Page contains 'не знайдено' - word not found");
                }
                
                // Sample of HTML
                Console.WriteLine($"\nFirst 500 chars of HTML:\n{html.Substring(0, Math.Min(500, html.Length))}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    
    static string TransliterateCyrillicToLatin(string input)
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
}
