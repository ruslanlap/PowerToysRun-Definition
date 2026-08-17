using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.Definition
{
    /// <summary>
    /// "Did you mean...?" suggestions via the free Datamuse API (no key required)
    /// when dictionary lookup returns zero results.
    /// </summary>
    internal class SuggestionProvider
    {
        private const string DatamuseApiBase = "https://api.datamuse.com/words?sp=";
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public SuggestionProvider(HttpClient httpClient, string apiKey = "")
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = apiKey;
        }

        public async Task<List<string>> GetSuggestionsAsync(string word, int max, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(word) || word.Length > 50 || max <= 0)
            {
                return new List<string>();
            }

            try
            {
                // sp=word* — fuzzy spell check; max keeps it fast
                var url = $"{DatamuseApiBase}{Uri.EscapeDataString(word)}&max={Math.Min(max, 25)}";
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                if (!string.IsNullOrWhiteSpace(_apiKey))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
                }
                using var response = await _httpClient.SendAsync(request, token);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<string>();
                }

                await using var stream = await response.Content.ReadAsStreamAsync(token);
                using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: token);
                return doc.RootElement.EnumerateArray()
                    .Select(e => e.TryGetProperty("word", out var w) ? w.GetString() : null)
                    .Where(w => !string.IsNullOrWhiteSpace(w) && !string.Equals(w, word, StringComparison.OrdinalIgnoreCase))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Take(max)
                    .ToList()!;
            }
            catch (Exception)
            {
                // Suggestions are best-effort; never surface errors to the user
                return new List<string>();
            }
        }
    }
}
