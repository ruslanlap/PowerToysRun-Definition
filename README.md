# 🔍 PowerToys Run: Definition Plugin

<div align="center">
  <img src="data/definition.logo.png" alt="Definition Plugin Logo" width="128" height="128">
</div>

<div align="center">
  <h1>Definition</h1>
  <p>Lookup word definitions, phonetics, and synonyms directly in PowerToys Run.</p>
  <img src="data/demo-definition.gif" alt="Definition Plugin Demo" width="650">
</div>

<div align="center">
  <!-- Badges -->
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml">
    <img src="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml/badge.svg" alt="Build Status">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/v/release/ruslanlap/PowerToysRun-Definition?label=latest" alt="Latest Release">
  </a>
  <img src="https://img.shields.io/maintenance/yes/2025" alt="Maintenance">
  <img src="https://img.shields.io/badge/C%23-.NET-512BD4" alt="C# .NET">
  <img src="https://img.shields.io/badge/version-v0.90.1-brightgreen" alt="Version">
  <img src="https://img.shields.io/badge/PRs-welcome-brightgreen.svg" alt="PRs Welcome">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/stargazers">
    <img src="https://img.shields.io/github/stars/ruslanlap/PowerToysRun-Definition" alt="GitHub stars">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/issues">
    <img src="https://img.shields.io/github/issues/ruslanlap/PowerToysRun-Definition" alt="GitHub issues">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/downloads/ruslanlap/PowerToysRun-Definition/total" alt="GitHub all releases">
  </a>
  <img src="https://img.shields.io/badge/Made%20with-❤️-red" alt="Made with Love">
  <img src="https://img.shields.io/badge/Awesome-Yes-orange" alt="Awesome">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/v/release/ruslanlap/PowerToysRun-Definition?style=for-the-badge" alt="Latest Release">
  </a>
  <img src="https://img.shields.io/badge/PowerToys-Compatible-blue" alt="PowerToys Compatible">
  <img src="https://img.shields.io/badge/platform-Windows-lightgrey" alt="Platform">
  <a href="https://opensource.org/licenses/MIT">
    <img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="License">
  </a>
</div>

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v0.90.1/Definition-v0.90.1-x64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-x64-blue?style=for-the-badge&logo=github" alt="Download x64">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v0.90.1/Definition-v0.90.1-arm64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-ARM64-blue?style=for-the-badge&logo=github" alt="Download ARM64">
  </a>
</div>

## 📋 Table of Contents

- [📋 Overview](#-overview)
- [✨ Features](#-features)
- [🎬 Demo](#-demo)
- [🚀 Installation](#-installation)
- [🔧 Usage](#-usage)
- [📁 Data Storage](#-data-storage)
- [🛠️ Building from Source](#️-building-from-source)
- [📊 Project Structure](#-project-structure)
- [🤝 Contributing](#-contributing)
- [❓ FAQ](#-faq)
- [🧑‍💻 Tech Stack](#-tech-stack)
- [🌐 Localization](#-localization)
- [📸 Screenshots](#-screenshots)
- [📄 License](#-license)
- [🙏 Acknowledgements](#-acknowledgements)
- [☕ Support](#-support)

## 📋 Overview

Definition is a plugin for [Microsoft PowerToys Run](https://github.com/microsoft/PowerToys) that allows you to quickly lookup word definitions, phonetics, and synonyms without leaving your keyboard. Simply type `def <word>` to fetch definitions from dictionaryapi.dev.

<div align="center">
  <img src="data/demo-definition-2.gif" alt="Lookup word definitions" width="650">
</div>

## ✨ Features

- 🔍 **Instant Definitions**: Get definitions in real-time via `dictionaryapi.dev`.
- 🔊 **Pronunciation Audio**: Play phonetic audio directly from your results.
- 📚 **Phonetics & Synonyms**: View phonetic spelling and synonyms (if available).
- ⏱️ **Delayed Execution**: Shows loading indicator before fetching results.
- 💾 **Caching**: In-memory cache for repeat lookups to improve performance.
- 🌓 **Theme Awareness**: Automatically switches icons for light/dark mode.
- 📋 **Context Menu**: Copy definitions or play pronunciation via right-click or keyboard shortcuts.
- ⚙️ **Configurable**: Easily extend or modify via plugin.json.

## 🎬 Demo

<div align="center">
  <img src="data/demo-definition.gif" alt="Definition Plugin Demo" width="650">
</div>

## 🚀 Installation

### Prerequisites

- [PowerToys Run](https://github.com/microsoft/PowerToys/releases) installed
- Windows 10 (build 22621) or later

### Quick Install

1. Download the latest release from the [Releases page](https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest).
2. Extract the ZIP to:
   ```
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```
3. Restart PowerToys.
4. Open PowerToys Run (`Alt + Space`) and type `def <word>`.

## 🔧 Usage

1. Activate PowerToys Run (`Alt + Space`).
2. Type:
   - `def` to see instructions.
   - `def <word>` to lookup definitions.
3. Press <kbd>Enter</kbd> to fetch results.
4. Use <kbd>Ctrl + C</kbd> to copy a definition.
5. Right-click a result to play pronunciation.

## 📁 Data Storage

All settings are stored in the standard PowerToys settings file (no additional data files created).

## 🛠️ Building from Source

```bash
git clone https://github.com/ruslanlap/PowerToysRun-Definition.git
cd PowerToysRun-Definition/Definition
dotnet build
# To package:
dotnet publish -c Release -r win-x64 --output ./publish
zip -r Definition-v0.90.1-x64.zip ./publish
```

## 📊 Project Structure

```
PowerToysRun-Definition/
├── data/                            # Plugin assets (icons, demos)
│   ├── definition.dark.png
│   ├── definition.logo.png
│   ├── demo-definition.gif
│   └── demo-definition-2.gif
├── Definition/                      # Plugin source
│   ├── Community.PowerToys.Run.Plugin.Definition/
│   │   ├── Images/
│   │   │   ├── definition.dark.png
│   │   │   └── definition.light.png
│   │   ├── Main.cs
│   │   └── plugin.json
│   └── Community.PowerToys.Run.Plugin.Definition.csproj
└── README.md
```

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

Please make sure to update tests as appropriate.

### Contributors

- [ruslanlap](https://github.com/ruslanlap) - Project creator and maintainer

## ❓ FAQ

<details>
<summary><b>Does the plugin require internet access?</b></summary>
<p>Yes, the plugin needs internet access to fetch definitions from dictionaryapi.dev.</p>
</details>

<details>
<summary><b>How do I change the plugin's theme?</b></summary>
<p>The plugin automatically adapts to your PowerToys theme (light/dark).</p>
</details>

<details>
<summary><b>Are definitions cached?</b></summary>
<p>Yes, definitions are cached in memory during the current session to improve performance.</p>
</details>

<details>
<summary><b>Can I customize the dictionary source?</b></summary>
<p>Not in the current version, but this may be added in future updates.</p>
</details>

## 🧑‍💻 Tech Stack

- C# / .NET 9.0
- PowerToys Run API
- HttpClient for API requests
- WPF for UI components
- GitHub Actions for CI/CD

## 🌐 Localization

Currently, the plugin UI is in English. Localization support is planned for future releases.

## 📸 Screenshots

<div align="center">
  <figure>
    <img src="data/demo1.png" width="350" alt="Demo: Word Definition">
    <figcaption>Word Definition</figcaption>
  </figure>
  <figure>
    <img src="data/demo2.png" width="350" alt="Demo: Phonetics Display">
    <figcaption>Phonetics Display</figcaption>
  </figure>
  <figure>
    <img src="data/demo3.png" width="350" alt="Demo: Context Menu">
    <figcaption>Context Menu</figcaption>
  </figure>
</div>

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgements

- [Microsoft PowerToys](https://github.com/microsoft/PowerToys) team for the amazing launcher
- [dictionaryapi.dev](https://dictionaryapi.dev/) for providing the free dictionary API
- All contributors who have helped improve this plugin

## ☕ Support

If you find this plugin useful and would like to support its development, you can buy me a coffee:

[![Buy me a coffee](https://img.shields.io/badge/Buy%20me%20a%20coffee-☕️-FFDD00?style=for-the-badge&logo=buy-me-a-coffee)](https://ruslanlap.github.io/ruslanlap_buymeacoffe/)

---

<div align="center">
  <sub>Made with ❤️ by <a href="https://github.com/ruslanlap">ruslanlap</a></sub>
</div>
