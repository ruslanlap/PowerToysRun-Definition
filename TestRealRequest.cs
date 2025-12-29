using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text;
using System.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        string word = "слово";
        if (args.Length > 0) word = args[0];

        Console.WriteLine($"Testing lookup for: {word}");
        
        using var client = new HttpClient();
        // client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

        var provider = new UkrainianDictionaryProvider(client);
        
        try 
        {
            var entries = await provider.LookupAsync(word);
            
            if (entries.Count == 0)
            {
                Console.WriteLine("❌ No entries found.");
            }
            else
            {
                Console.WriteLine($"✅ Found {entries.Count} entries:");
                foreach (var entry in entries)
                {
                    Console.WriteLine($"Word: {entry.Word}");
                    foreach (var meaning in entry.Meanings)
                    {
                        Console.WriteLine($"  [{meaning.PartOfSpeech}]");
                        foreach (var def in meaning.Definitions)
                        {
                            Console.WriteLine($"    - {def.Definition.Substring(0, Math.Min(100, def.Definition.Length))}...");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex}");
        }
    }
}

// Minimal mock of the classes needed
public class UkrainianDictionaryProvider
{
    private readonly HttpClient _httpClient;
    public string UkrainianApiEndpoint = "https://sum.in.ua/s/";

    public UkrainianDictionaryProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DictionaryEntry>> LookupAsync(string word)
    {
        var transliteratedWord = TransliterateCyrillicToLatin(word);
        var requestUrl = $"{UkrainianApiEndpoint}{Uri.EscapeDataString(transliteratedWord)}";
        
        Console.WriteLine($"Requesting: {requestUrl}");

        using var response = await _httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"HTTP Error: {response.StatusCode}");
            return new List<DictionaryEntry>();
        }

        // Fix encoding if needed (sum.in.ua is UTF-8 usually)
        var bytes = await response.Content.ReadAsByteArrayAsync();
        var encoding = Encoding.UTF8; // Force UTF8 just to be safe
        var html = encoding.GetString(bytes); 
        
        // Debug: Print a snippet of HTML
        // Console.WriteLine("HTML Snippet: " + html.Substring(0, 500));

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // LOGIC FROM ACTUAL PLUGIN:
        var articleNodes = doc.DocumentNode.SelectNodes("//div[@itemprop='articleBody']");
        
        if (articleNodes == null || articleNodes.Count == 0)
        {
            Console.WriteLine("DEBUG: articleBody not found, trying fallback...");
            articleNodes = doc.DocumentNode.SelectNodes("//div[@id='article']");
        }

        if (articleNodes == null) 
        {
            Console.WriteLine("DEBUG: No article nodes found at all.");
            return new List<DictionaryEntry>();
        }
        
        Console.WriteLine($"DEBUG: Found {articleNodes.Count} article nodes.");

        var entries = new List<DictionaryEntry>();
        foreach (var node in articleNodes)
        {
            var entry = new DictionaryEntry
            {
                Word = word,
            };

            var cleanText = node.InnerText.Trim();
            // Decode HTML entities (e.g. &nbsp;)
            cleanText = System.Net.WebUtility.HtmlDecode(cleanText);

            var meaning = new Meaning
            {
                PartOfSpeech = "unknown",
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
        if (string.IsNullOrEmpty(input)) return input;
        var transliteration = new Dictionary<char, string>
        {
            {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'ґ', "g"}, {'д', "d"},
            {'е', "e"}, {'є', "je"}, {'ж', "zh"}, {'з', "z"}, {'и', "y"}, {'і', "i"},
            {'ї', "ji"}, {'й', "j"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
            {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"}, {'у', "u"},
            {'ф', "f"}, {'х', "h"}, {'ц', "c"}, {'ч', "ch"}, {'ш', "sh"}, {'щ', "shh"},
            {'ь', "j"}, {'ю', "ju"}, {'я', "ja"},
        };
        // Quick version for test (case insensitive handling simplified)
        var sb = new StringBuilder();
        foreach(var c in input.ToLower())
        {
            if(transliteration.TryGetValue(c, out var val)) sb.Append(val);
            else sb.Append(c);
        }
        return sb.ToString();
    }
}

public class DictionaryEntry
{
    public string Word { get; set; }
    public List<Meaning> Meanings { get; set; } = new List<Meaning>();
}
public class Meaning
{
    public string PartOfSpeech { get; set; }
    public List<DefinitionItem> Definitions { get; set; } = new List<DefinitionItem>();
}
public class DefinitionItem
{
    public string Definition { get; set; }
}
