# Definition for Command Palette

A **PowerToys Command Palette** extension that provides instant word definitions, phonetics, synonyms, antonyms, and usage examples — directly from the Command Palette.

Built as a CmdPal extension for the PowerToys Command Palette.

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

## Deployment Guide

### Prerequisites

- **Windows 10** (build 19041 or later) or **Windows 11**
- **PowerToys** v0.70.0+ with Command Palette enabled
- **.NET 9.0 SDK** (for building from source)
- **Visual Studio 2022** (recommended) with:
  - .NET desktop development workload
  - Windows App SDK / WinUI 3 tooling
- **Developer Mode** enabled on Windows (Settings > Update & Security > For developers)

### Method 1: Deploy via Visual Studio (Recommended)

This is the simplest method for development and personal use.

1. **Clone the repository:**

   ```bash
   git clone https://github.com/ruslanlap/PowerToysRun-Definition.git
   cd PowerToysRun-Definition/CmdPal
   ```

2. **Open the solution** in Visual Studio 2022:

   ```
   CmdPal/CmdPal-Definition.sln
   ```

3. **Set the platform** to `x64` or `ARM64` (match your system architecture).

4. **Set build configuration** to `Release` (or `Debug` for development).

5. **Deploy the project:**
   - Right-click the `DefinitionExtension` project in Solution Explorer
   - Select **Deploy**
   - Visual Studio will build the MSIX package and register it with Windows

6. **Restart PowerToys** (right-click tray icon > Restart).

7. Open **Command Palette** and the "Definition" extension should appear.

### Method 2: Build and Deploy from Command Line

1. **Clone and build:**

   ```bash
   git clone https://github.com/ruslanlap/PowerToysRun-Definition.git
   cd PowerToysRun-Definition
   dotnet build CmdPal/CmdPal-Definition.sln -c Release -p:Platform="x64"
   ```

   For ARM64:
   ```bash
   dotnet build CmdPal/CmdPal-Definition.sln -c Release -p:Platform="ARM64"
   ```

2. **Create the MSIX package:**

   ```bash
   dotnet publish CmdPal/CmdPal-Definition.sln -c Release -p:Platform="x64"
   ```

3. **Install the MSIX package:**

   Navigate to the build output directory and find the generated `.msix` file. Double-click it to install, or use PowerShell:

   ```powershell
   Add-AppPackage -Path "path\to\DefinitionExtension.msix"
   ```

4. **Restart PowerToys** for the extension to be detected.

### Method 3: Sideload with Self-Signed Certificate

For distributing to other machines without the Microsoft Store:

1. **Generate a self-signed certificate** (run PowerShell as Administrator):

   ```powershell
   New-SelfSignedCertificate `
     -Type Custom `
     -Subject "CN=A1B2C3D4-E5F6-7890-ABCD-EF1234567890" `
     -KeyUsage DigitalSignature `
     -FriendlyName "CmdPal Definition Dev Cert" `
     -CertStoreLocation "Cert:\CurrentUser\My" `
     -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")
   ```

   > **Note:** The Subject (`CN=...`) must match the `Publisher` attribute in `Package.appxmanifest`.

2. **Export the certificate:**

   ```powershell
   $cert = Get-ChildItem "Cert:\CurrentUser\My" | Where-Object { $_.FriendlyName -eq "CmdPal Definition Dev Cert" }
   Export-Certificate -Cert $cert -FilePath "DefinitionExtension.cer"
   ```

3. **On the target machine**, install the certificate into **Trusted Root Certification Authorities**:

   ```powershell
   Import-Certificate -FilePath "DefinitionExtension.cer" -CertStoreLocation "Cert:\LocalMachine\Root"
   ```

4. **Build and sign the MSIX** in Visual Studio:
   - Open project properties > Packaging > Package Signing
   - Select the certificate you created
   - Build in Release mode

5. **Install the signed MSIX** on the target machine:

   ```powershell
   Add-AppPackage -Path "DefinitionExtension.msix"
   ```

### Verifying the Deployment

1. Open PowerToys and ensure **Command Palette** is enabled in settings.
2. Launch Command Palette (default: `Win + Ctrl + T` or the configured shortcut).
3. The "Definition" extension should appear in the list of available commands.
4. Select it and type any word to look up its definition.

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Extension not visible in Command Palette | Restart PowerToys. Ensure Developer Mode is enabled. Check that the MSIX is installed (`Get-AppPackage *Definition*`). |
| Build fails with SDK errors | Install Windows App SDK and ensure `net9.0-windows10.0.26100.38` target framework is available. |
| Certificate trust errors when sideloading | Ensure the `.cer` is installed in `Cert:\LocalMachine\Root` on the target machine. |
| MSIX install fails | Enable Developer Mode or sideloading in Windows Settings. Run `Add-AppPackage` with `-AllowUnsigned` for unsigned dev builds. |
| API errors / no results | Check internet connection. The Free Dictionary API requires no API key but needs network access. |

### Uninstalling

To remove the extension:

```powershell
Get-AppPackage *DefinitionForCommandPalette* | Remove-AppPackage
```

Or uninstall via Windows Settings > Apps > Installed apps.

## API

Uses the [Free Dictionary API](https://dictionaryapi.dev/) — no API key required.

Endpoint: `https://api.dictionaryapi.dev/api/v2/entries/{lang}/{word}`

## Requirements

- Windows 10 (19041+)
- PowerToys with Command Palette
- .NET 9.0

## License

MIT
