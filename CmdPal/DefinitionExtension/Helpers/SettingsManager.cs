// Copyright (c) ruslanlap
// Licensed under the MIT license.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.IO;

namespace DefinitionExtension.Helpers;

public class SettingsManager : JsonSettingsManager
{
    private readonly TextSetting _apiEndpoint = new("apiEndpoint",
        "https://api.dictionaryapi.dev/api/v2/entries/en/")
    {
        Label = "API Endpoint",
        Description = "Dictionary API endpoint URL",
    };

    private readonly ChoiceSetSetting _maxResults = new("maxResults",
    [
        new ChoiceSetSetting.Choice("3", "3"),
        new ChoiceSetSetting.Choice("5", "5"),
        new ChoiceSetSetting.Choice("10", "10"),
        new ChoiceSetSetting.Choice("All", "99"),
    ])
    {
        Label = "Max Results Per Meaning",
        Description = "Maximum number of definition results to show per part of speech",
        Value = "3",
    };

    private readonly ToggleSetting _showExamples = new("showExamples", true)
    {
        Label = "Show Examples",
        Description = "Show usage examples in results",
    };

    private readonly ToggleSetting _showSynonyms = new("showSynonyms", true)
    {
        Label = "Show Synonyms",
        Description = "Show synonyms in results",
    };

    private readonly ToggleSetting _showAntonyms = new("showAntonyms", true)
    {
        Label = "Show Antonyms",
        Description = "Show antonyms in results",
    };

    private readonly ToggleSetting _showPhonetics = new("showPhonetics", true)
    {
        Label = "Show Phonetics",
        Description = "Show phonetic transcription in result titles",
    };

    private readonly ChoiceSetSetting _language = new("language",
    [
        new ChoiceSetSetting.Choice("English", "en"),
        new ChoiceSetSetting.Choice("Spanish", "es"),
        new ChoiceSetSetting.Choice("French", "fr"),
        new ChoiceSetSetting.Choice("German", "de"),
        new ChoiceSetSetting.Choice("Italian", "it"),
        new ChoiceSetSetting.Choice("Portuguese", "pt-BR"),
        new ChoiceSetSetting.Choice("Japanese", "ja"),
        new ChoiceSetSetting.Choice("Korean", "ko"),
        new ChoiceSetSetting.Choice("Turkish", "tr"),
        new ChoiceSetSetting.Choice("Arabic", "ar"),
        new ChoiceSetSetting.Choice("Hindi", "hi"),
    ])
    {
        Label = "Language",
        Description = "Dictionary language for lookups",
        Value = "en",
    };

    public string ApiEndpoint
    {
        get
        {
            var lang = _language.Value ?? "en";
            var customEndpoint = _apiEndpoint.Value ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(customEndpoint) &&
                !customEndpoint.StartsWith("https://api.dictionaryapi.dev"))
            {
                return customEndpoint;
            }
            return $"https://api.dictionaryapi.dev/api/v2/entries/{lang}/";
        }
    }

    public int MaxResultsPerMeaning => int.TryParse(_maxResults.Value, out var v) ? v : 3;
    public bool ShowExamples => _showExamples.Value;
    public bool ShowSynonyms => _showSynonyms.Value;
    public bool ShowAntonyms => _showAntonyms.Value;
    public bool ShowPhonetics => _showPhonetics.Value;
    public string Language => _language.Value ?? "en";

    internal Pages.DefinitionPage? ExtensionHomePage { get; set; }

    internal static string SettingsJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("Definition");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "settings.json");
    }

    public SettingsManager()
    {
        FilePath = SettingsJsonPath();
        Settings.Add(_language);
        Settings.Add(_maxResults);
        Settings.Add(_showExamples);
        Settings.Add(_showSynonyms);
        Settings.Add(_showAntonyms);
        Settings.Add(_showPhonetics);
        Settings.Add(_apiEndpoint);

        try
        {
            LoadSettings();
        }
        catch (FileNotFoundException)
        {
            SaveSettings();
        }

        Settings.SettingsChanged += (s, a) =>
        {
            SaveSettings();
            CommandResult.GoBack();
            ExtensionHomePage?.ReloadExtensionState();
        };
    }
}
