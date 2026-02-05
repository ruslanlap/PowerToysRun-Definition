// Copyright (c) ruslanlap
// Licensed under the MIT license.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using DefinitionExtension.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DefinitionExtension.Pages;

internal sealed partial class DefinitionPage : DynamicListPage
{
    private List<DefinitionListItem> _items = new();
    private readonly DictionaryService _dictionaryService;
    private readonly SettingsManager _settingsManager;
    private CancellationTokenSource? _currentSearchCts;
    private string _lastSearch = string.Empty;
    private bool _isQueryRunning;

    private readonly IconInfo _logoIcon = new("\uE82D");

    public DefinitionPage(SettingsManager settingsManager, DictionaryService dictionaryService)
    {
        _settingsManager = settingsManager;
        _dictionaryService = dictionaryService;
        settingsManager.ExtensionHomePage = this;

        Icon = _logoIcon;
        Title = "Definition";
        Name = "Open";
        ShowDetails = true;
        PlaceholderText = "Type a word to look up its definition...";

        ReloadExtensionState();
    }

    public void ReloadExtensionState()
    {
        var langName = _settingsManager.Language switch
        {
            "en" => "English",
            "es" => "Spanish",
            "fr" => "French",
            "de" => "German",
            "it" => "Italian",
            "pt-BR" => "Portuguese",
            "ja" => "Japanese",
            "ko" => "Korean",
            "tr" => "Turkish",
            "ar" => "Arabic",
            "hi" => "Hindi",
            _ => "English"
        };

        Title = $"Definition ({langName})";
        EmptyContent = new CommandItem(new NoOpCommand())
        {
            Icon = _logoIcon,
            Title = "Word Definition Lookup",
            Subtitle = $"Type a word to search the {langName} dictionary",
        };
        RaiseItemsChanged(_items.Count);
    }

    public override async void UpdateSearchText(string oldSearch, string newSearch)
    {
        try
        {
            if (_lastSearch == newSearch)
                return;

            _lastSearch = newSearch;
            var trimmedSearch = newSearch.Trim();

            if (string.IsNullOrEmpty(trimmedSearch))
            {
                _items.Clear();
                RaiseItemsChanged(0);
                return;
            }

            // Debounce: wait briefly before sending the request
            _currentSearchCts?.Cancel();
            _currentSearchCts = new CancellationTokenSource();
            var token = _currentSearchCts.Token;

            await Task.Delay(300, token);

            if (token.IsCancellationRequested)
                return;

            await UpdateListAsync(trimmedSearch, token);
        }
        catch (OperationCanceledException)
        {
            // Expected from debouncing
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Definition CmdPal] Search error: {ex.Message}");
            HandleError("Error", "An unexpected error occurred during lookup.");
        }
        finally
        {
            IsLoading = false;
            _isQueryRunning = false;
        }
    }

    private async Task UpdateListAsync(string word, CancellationToken cancellationToken)
    {
        if (_isQueryRunning)
            return;

        _isQueryRunning = true;
        _items.Clear();
        IsLoading = true;
        RaiseItemsChanged(0);

        var entries = await _dictionaryService.LookupAsync(
            word,
            _settingsManager.ApiEndpoint,
            cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            return;

        if (entries.Count == 0)
        {
            HandleError(
                $"No definitions found for \"{word}\"",
                "Check spelling or try another word.");
            return;
        }

        var items = BuildResultItems(entries, word);
        _items.Clear();
        _items.AddRange(items);
        IsLoading = false;
        _isQueryRunning = false;
        Title = $"\"{word}\" — {_items.Count} results";
        RaiseItemsChanged(_items.Count);
    }

    private List<DefinitionListItem> BuildResultItems(
        List<DictionaryEntry> entries,
        string searchWord)
    {
        var items = new List<DefinitionListItem>();

        foreach (var entry in entries.Where(e => e != null))
        {
            // Get phonetic info
            var phoneticText = entry.Phonetics?
                .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p?.Text))?.Text
                ?? entry.Phonetic;

            var audioUrl = entry.Phonetics?
                .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p?.Audio))?.Audio;

            var sourceUrl = entry.SourceUrls?.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

            // Word header with phonetics
            var headerTitle = entry.Word;
            if (_settingsManager.ShowPhonetics && !string.IsNullOrWhiteSpace(phoneticText))
            {
                headerTitle = $"{entry.Word}  {phoneticText}";
            }

            foreach (var meaning in entry.Meanings?.Where(m => m != null) ?? Enumerable.Empty<Meaning>())
            {
                var partOfSpeech = meaning.PartOfSpeech ?? "unknown";
                var posTag = new Tag(partOfSpeech);

                // Definitions
                var definitions = meaning.Definitions?
                    .Where(d => d != null && !string.IsNullOrWhiteSpace(d.Definition))
                    .Take(_settingsManager.MaxResultsPerMeaning)
                    ?? Enumerable.Empty<DefinitionItem>();

                foreach (var def in definitions)
                {
                    items.Add(new DefinitionListItem(
                        title: $"{headerTitle} ({partOfSpeech})",
                        subtitle: def.Definition,
                        itemType: DefinitionItemType.Definition,
                        textToCopy: def.Definition,
                        audioUrl: audioUrl,
                        sourceUrl: sourceUrl,
                        word: entry.Word,
                        tags: new[] { posTag }));

                    // Examples
                    if (_settingsManager.ShowExamples && !string.IsNullOrWhiteSpace(def.Example))
                    {
                        items.Add(new DefinitionListItem(
                            title: $"Example ({partOfSpeech})",
                            subtitle: $"\"{def.Example}\"",
                            itemType: DefinitionItemType.Example,
                            textToCopy: def.Example,
                            sourceUrl: sourceUrl,
                            word: entry.Word,
                            tags: new[] { new Tag("example") }));
                    }
                }

                // Synonyms
                if (_settingsManager.ShowSynonyms)
                {
                    var synonyms = meaning.Synonyms?
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList();

                    if (synonyms?.Count > 0)
                    {
                        var synonymsText = string.Join(", ", synonyms);
                        items.Add(new DefinitionListItem(
                            title: $"Synonyms ({partOfSpeech})",
                            subtitle: synonymsText,
                            itemType: DefinitionItemType.Synonym,
                            textToCopy: synonymsText,
                            sourceUrl: sourceUrl,
                            word: entry.Word,
                            tags: new[] { new Tag("synonyms") }));
                    }
                }

                // Antonyms
                if (_settingsManager.ShowAntonyms)
                {
                    var antonyms = meaning.Antonyms?
                        .Where(a => !string.IsNullOrWhiteSpace(a))
                        .ToList();

                    if (antonyms?.Count > 0)
                    {
                        var antonymsText = string.Join(", ", antonyms);
                        items.Add(new DefinitionListItem(
                            title: $"Antonyms ({partOfSpeech})",
                            subtitle: antonymsText,
                            itemType: DefinitionItemType.Antonym,
                            textToCopy: antonymsText,
                            sourceUrl: sourceUrl,
                            word: entry.Word,
                            tags: new[] { new Tag("antonyms") }));
                    }
                }
            }
        }

        return items;
    }

    private void HandleError(string title, string message)
    {
        IsLoading = false;
        _isQueryRunning = false;
        EmptyContent = new CommandItem(new NoOpCommand())
        {
            Icon = new IconInfo("\uE946"),
            Title = title,
            Subtitle = message,
        };
        _items.Clear();
        RaiseItemsChanged(0);
    }

    public override IListItem[] GetItems() => _items.ToArray();
}
