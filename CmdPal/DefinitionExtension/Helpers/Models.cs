// Copyright (c) ruslanlap
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DefinitionExtension.Helpers;

public class DictionaryEntry
{
    [JsonPropertyName("word")]
    public string Word { get; set; } = string.Empty;

    [JsonPropertyName("phonetic")]
    public string? Phonetic { get; set; }

    [JsonPropertyName("phonetics")]
    public List<PhoneticInfo> Phonetics { get; set; } = new();

    [JsonPropertyName("meanings")]
    public List<Meaning> Meanings { get; set; } = new();

    [JsonPropertyName("license")]
    public LicenseInfo? License { get; set; }

    [JsonPropertyName("sourceUrls")]
    public List<string> SourceUrls { get; set; } = new();
}

public class PhoneticInfo
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("audio")]
    public string? Audio { get; set; }

    [JsonPropertyName("sourceUrl")]
    public string? SourceUrl { get; set; }

    [JsonPropertyName("license")]
    public LicenseInfo? License { get; set; }
}

public class Meaning
{
    [JsonPropertyName("partOfSpeech")]
    public string PartOfSpeech { get; set; } = string.Empty;

    [JsonPropertyName("definitions")]
    public List<DefinitionItem> Definitions { get; set; } = new();

    [JsonPropertyName("synonyms")]
    public List<string> Synonyms { get; set; } = new();

    [JsonPropertyName("antonyms")]
    public List<string> Antonyms { get; set; } = new();
}

public class DefinitionItem
{
    [JsonPropertyName("definition")]
    public string Definition { get; set; } = string.Empty;

    [JsonPropertyName("example")]
    public string? Example { get; set; }

    [JsonPropertyName("synonyms")]
    public List<string> Synonyms { get; set; } = new();

    [JsonPropertyName("antonyms")]
    public List<string> Antonyms { get; set; } = new();
}

public class LicenseInfo
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

[JsonSerializable(typeof(List<DictionaryEntry>))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public partial class DictionaryEntryContext : JsonSerializerContext
{
}
