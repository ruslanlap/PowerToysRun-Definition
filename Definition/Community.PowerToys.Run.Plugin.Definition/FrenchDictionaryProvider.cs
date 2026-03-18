using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class FrenchDictionaryProvider : IDictionaryProvider
    {
        private readonly HttpClient _httpClient;
        public string LanguageCode => "fr";
        public string DisplayName => "Français (Free Dictionary API)";

        public FrenchDictionaryProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token)
        {
            var requestUrl = $"{ConfigurationManager.Configuration.FrenchApiEndpoint}{Uri.EscapeDataString(word)}";

            using var response = await _httpClient.GetAsync(requestUrl, token);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<DictionaryEntry>();
            }

            if (!response.IsSuccessStatusCode)
            {
                var httpEx = new HttpRequestException($"HTTP {(int)response.StatusCode} {response.StatusCode}");
                httpEx.Data["StatusCode"] = response.StatusCode;
                throw httpEx;
            }

            await using var jsonStream = await response.Content.ReadAsStreamAsync(token);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var entries = await JsonSerializer.DeserializeAsync<List<DictionaryEntry>>(jsonStream, options, token);

            return entries ?? new List<DictionaryEntry>();
        }
    }
}
