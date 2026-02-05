# Definition for Command Palette

A **PowerToys Command Palette** extension that provides instant word definitions, phonetics, synonyms, antonyms, and usage examples — directly from the Command Palette.

Built as a CmdPal extension following the same architecture as [CmdPal-VideoDownloader](https://github.com/DevLGuilherme/CmdPal-VideoDownloader).

## Features

- **Instant Definitions** — Type any word to get definitions from the Free Dictionary API
- **Multi-Language Support** — English, Spanish, French, German, Italian, Portuguese, Japanese, Korean, Turkish, Arabic, Hindi
- **Phonetics** — See phonetic transcriptions alongside words
- **Synonyms & Antonyms** — Discover related words at a glance
- **Usage Examples** — See real-world usage examples
- **Copy to Clipboard** — Click any result to copy the text
- **Open in Wiktionary** — Right-click context menu to open in Wiktionary
- **Configurable** — Settings page for language, result count, and display options
- **Smart Caching** — Results cached in memory for fast repeat lookups
- **Debounced Search** — 300ms debounce prevents excessive API calls while typing

## Architecture

```
CmdPal/
├── CmdPal-Definition.sln
└── DefinitionExtension/
    ├── DefinitionExtension.cs          # IExtension entry point (COM server)
    ├── DefinitionCommandsProvider.cs   # CommandProvider with top-level commands
    ├── Program.cs                      # Main entry point
    ├── Package.appxmanifest            # MSIX manifest with CmdPal registration
    ├── Pages/
    │   ├── DefinitionPage.cs           # DynamicListPage — main search UI
    │   └── DefinitionListItem.cs       # ListItem for each result
    ├── Helpers/
    │   ├── DictionaryService.cs        # HTTP client with caching
    │   ├── SettingsManager.cs          # JsonSettingsManager with CmdPal settings UI
    │   ├── Models.cs                   # JSON-serializable data models
    │   └── DefinitionExtensionHost.cs  # Extension host singleton
    ├── Assets/                         # MSIX tile assets
    ├── Strings/en-US/                  # Localization resources
    └── Properties/                     # Launch settings
```

## How It Works

1. User opens Command Palette and selects "Definition"
2. User types a word in the search box
3. After a 300ms debounce, the extension queries the Free Dictionary API
4. Results are displayed as a dynamic list with:
   - **Definitions** grouped by part of speech (noun, verb, adjective, etc.)
   - **Examples** shown below relevant definitions
   - **Synonyms** and **Antonyms** listed per part of speech
5. Clicking a result copies the text to clipboard
6. Right-click context menu provides: Copy, Open in Wiktionary, Open Source

## Building

```bash
dotnet build CmdPal/CmdPal-Definition.sln -c Release -p:Platform="x64"
```

## API

Uses the [Free Dictionary API](https://dictionaryapi.dev/) — no API key required.

Endpoint: `https://api.dictionaryapi.dev/api/v2/entries/{lang}/{word}`

## Requirements

- Windows 10 (19041+)
- PowerToys with Command Palette
- .NET 9.0

## License

MIT
