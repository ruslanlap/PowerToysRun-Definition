using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.Definition
{
    /// <summary>
    /// Chinese dictionary provider using an embedded CC-CEDICT database.
    /// Provides instant, offline results for Chinese characters (Simplified/Traditional) and Pinyin.
    /// </summary>
    internal class ChineseDictionaryProvider : IDictionaryProvider
    {
        public string LanguageCode => "zh";
        public string DisplayName => "中文 (CC-CEDICT Offline)";

        private static readonly Lazy<Dictionary<string, List<DictionaryEntry>>> _dictionaryData = new(LoadDictionary);

        public ChineseDictionaryProvider(System.Net.Http.HttpClient _)
        {
            // HttpClient is no longer needed but kept for interface compatibility
        }

        public Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(word))
                return Task.FromResult(new List<DictionaryEntry>());

            var normalizedWord = word.Trim().ToLowerInvariant();
            
            // Search in memory
            if (_dictionaryData.Value.TryGetValue(normalizedWord, out var entries))
            {
                return Task.FromResult(entries);
            }

            // Fallback: Partial match if no exact match (limit to top 10)
            var partialMatches = _dictionaryData.Value
                .Where(kvp => kvp.Key.Contains(normalizedWord))
                .Take(10)
                .SelectMany(kvp => kvp.Value)
                .ToList();

            return Task.FromResult(partialMatches);
        }

        private static Dictionary<string, List<DictionaryEntry>> LoadDictionary()
        {
            var data = new Dictionary<string, List<DictionaryEntry>>(StringComparer.OrdinalIgnoreCase);
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Community.PowerToys.Run.Plugin.Definition.Resources.cedict.txt.gz";

            try
            {
                using Stream stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[ChineseProvider] Resource not found: {resourceName}");
                    return data;
                }

                using GZipStream decompressionStream = new GZipStream(stream, CompressionMode.Decompress);
                using StreamReader reader = new StreamReader(decompressionStream, Encoding.UTF8);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    var entry = ParseLine(line);
                    if (entry == null) continue;

                    // Index by simplified
                    AddEntry(data, entry.Word, entry);
                    
                    // Index by traditional if different
                    var parts = line.Split(' ');
                    if (parts.Length > 0 && parts[0] != entry.Word)
                    {
                        AddEntry(data, parts[0], entry);
                    }
                    
                    // Index by pinyin (without tones for easier search)
                    if (!string.IsNullOrEmpty(entry.Phonetic))
                    {
                        var cleanPinyin = Regex.Replace(entry.Phonetic, @"\d", "").Replace(" ", "").ToLowerInvariant();
                        AddEntry(data, cleanPinyin, entry);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ChineseProvider] Error loading dictionary: {ex.Message}");
            }

            return data;
        }

        private static void AddEntry(Dictionary<string, List<DictionaryEntry>> data, string key, DictionaryEntry entry)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (!data.ContainsKey(key))
                data[key] = new List<DictionaryEntry>();
            data[key].Add(entry);
        }

        private static DictionaryEntry ParseLine(string line)
        {
            // Format: Traditional Simplified [pinyin] /english def 1/def 2/
            var match = Regex.Match(line, @"^(\S+)\s+(\S+)\s+\[([^\]]+)\]\s+/(.*)/$");
            if (!match.Success) return null;

            var traditional = match.Groups[1].Value;
            var simplified = match.Groups[2].Value;
            var pinyin = match.Groups[3].Value;
            var defPart = match.Groups[4].Value;

            var entry = new DictionaryEntry
            {
                Word = simplified,
                Phonetic = pinyin,
                SourceUrls = new List<string> { "https://www.mdbg.net/chinese/dictionary?page=cc-cedict" }
            };

            if (traditional != simplified)
            {
                entry.Word = $"{simplified} ({traditional})";
            }

            entry.Phonetics.Add(new Phonetic { Text = pinyin });

            var definitions = defPart.Split('/')
                .Select(d => d.Trim())
                .Where(d => !string.IsNullOrEmpty(d))
                .Select(d => new DefinitionItem { Definition = d })
                .ToList();

            entry.Meanings.Add(new Meaning
            {
                Definitions = definitions
            });

            return entry;
        }
    }
}
