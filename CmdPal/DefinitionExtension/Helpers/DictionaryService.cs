// Copyright (c) ruslanlap
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DefinitionExtension.Helpers;

public class DictionaryService
{
    private static readonly HttpClient _httpClient = CreateHttpClient();
    private readonly Dictionary<string, List<DictionaryEntry>> _cache = new();
    private const int MaxCacheSize = 100;

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
        return client;
    }

    public async Task<List<DictionaryEntry>> LookupAsync(
        string word,
        string apiEndpoint,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(word))
            return new List<DictionaryEntry>();

        var cacheKey = $"{apiEndpoint}:{word.ToLowerInvariant()}";

        if (_cache.TryGetValue(cacheKey, out var cached))
            return cached;

        try
        {
            var requestUrl = $"{apiEndpoint}{Uri.EscapeDataString(word.Trim())}";
            using var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new List<DictionaryEntry>();

            if (!response.IsSuccessStatusCode)
                return new List<DictionaryEntry>();

            var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
            var entries = JsonSerializer.Deserialize(
                jsonString,
                DictionaryEntryContext.Default.ListDictionaryEntry);

            var result = entries ?? new List<DictionaryEntry>();

            // Cache management
            if (_cache.Count >= MaxCacheSize)
            {
                var firstKey = _cache.Keys.First();
                _cache.Remove(firstKey);
            }
            _cache[cacheKey] = result;

            return result;
        }
        catch (OperationCanceledException)
        {
            return new List<DictionaryEntry>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Definition CmdPal] Lookup error for '{word}': {ex.Message}");
            return new List<DictionaryEntry>();
        }
    }

    public void ClearCache()
    {
        _cache.Clear();
    }
}
