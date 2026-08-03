# 🔍 PowerToys Run：Definition 词典插件

<div align="center">
  <img src="../data/definition.logo.png" alt="Definition Plugin Logo" width="128" height="128">
</div>

<div align="center">
  <h1>Definition</h1>
  <p>直接在 PowerToys Run 中查询单词释义、音标和同义词。</p>
  <img src="../data/demo-definition.gif" alt="Definition Plugin Demo" width="650">
</div>



<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml">
    <img src="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml/badge.svg" alt="Build Status">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/v/release/ruslanlap/PowerToysRun-Definition?label=latest" alt="Latest Release">
  </a>
  <img src="https://img.shields.io/badge/version-v1.5.1-brightgreen" alt="Version">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/stargazers">
    <img src="https://img.shields.io/github/stars/ruslanlap/PowerToysRun-Definition" alt="GitHub stars">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/issues">
    <img src="https://img.shields.io/github/issues/ruslanlap/PowerToysRun-Definition" alt="GitHub issues">
  </a>
  <a href="https://opensource.org/licenses/MIT">
    <img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="License">
      <img src="https://img.shields.io/badge/Made%20with-❤️-red" alt="Made with Love">
  <img src="https://img.shields.io/badge/Awesome-Yes-orange" alt="Awesome">
          <a href="https://github.com/hlaueriksson/awesome-powertoys-run-plugins">
    <img src="https://awesome.re/mentioned-badge.svg" alt="Mentioned in Awesome PowerToys Run Plugins">
  </a>
  <a href="https://winstall.app/apps/ruslanlap.DefinitionForCommandPalette">
    <img src="https://img.shields.io/badge/Install%20with-WinGet-blue.svg" alt="Install with WinGet">
  </a>
</div>

<div align="center">
  <a href="README.uk.md">
    <img src="https://img.shields.io/badge/docs-Українська-0057B7" alt="Docs Ukrainian">
  </a>
  <a href="../README.md">
    <img src="https://img.shields.io/badge/docs-English-2EA44F" alt="Docs English">
  </a>
  <a href="README.fr.md">
    <img src="https://img.shields.io/badge/docs-Français-1F6FEB" alt="Docs French">
  </a>
  <a href="README.it.md">
    <img src="https://img.shields.io/badge/docs-Italiano-009246" alt="Docs Italian">
  </a>
  <a href="README.zh.md">
    <img src="https://img.shields.io/badge/docs-中文-E34C26" alt="Docs Chinese">
  </a>
</div>

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-x64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-x64-blue?style=for-the-badge&logo=github" alt="Download x64">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-ARM64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-ARM64-blue?style=for-the-badge&logo=github" alt="Download ARM64">
  </a>
    <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/downloads/ruslanlap/PowerToysRun-Definition/total?style=for-the-badge&logo=github" alt="GitHub all releases">
  </a>
</div>

## 📋 目录

- [📋 概览](#-概览)
- [✨ 功能特性](#-功能特性)
- [🎬 演示](#-演示)
- [🚀 安装](#-安装)
- [🔧 使用方法](#-使用方法)
- [⚙️ 配置](#️-配置)
- [📁 数据存储](#-数据存储)
- [🛠️ 从源码构建](#️-从源码构建)
- [📊 项目结构](#-项目结构)
- [🤝 参与贡献](#-参与贡献)
- [❓ 常见问题](#-常见问题)
- [🧑‍💻 技术栈](#-技术栈)
- [🌐 支持语言](#-支持语言)
- [📸 截图](#-截图)
- [📄 许可证](#-许可证)
- [🙏 致谢](#-致谢)
- [☕ 支持](#-支持)
- [🆕 新功能（v1.5.1）](#-whats-new-v150)
- [🆕 新功能（v1.4.0）](#-whats-new-v140)
- [🆕 新功能（v1.3.3）](#-whats-new-v133)
- [🆕 新功能（v1.3.2）](#-whats-new-v132)
- [🆕 新功能（v1.3.1）](#-whats-new-v131)

## 🆕 新功能（v1.5.1）

- ⌨️ **子命令支持** — 无需查询完整释义即可快速访问特定的单词信息：
  - `def pronunciation <word>` / `def pron <word>` — 仅显示发音和音频
  - `def synonyms <word>` / `def syn <word>` — 仅显示同义词
  - `def antonyms <word>` / `def ant <word>` — 仅显示反义词
  - `def examples <word>` / `def ex <word>` — 仅显示用法示例
  - 默认：`def <word>` 显示全部（释义 + 音标 + 同义词 + 反义词 + 示例）
- 适用于所有支持的语言（英语、法语、意大利语、乌克兰语、中文）
- 可通过现有的 `ShowSynonymsInResults`、`ShowAntonymsInResults`、`ShowExamplesInResults` 设置进行配置

## 🆕 新功能（v1.5.1）

- 🇮🇹 **意大利语词典支持** — 通过 Wikizionario（`it.wiktionary.org`）添加了意大利语查询
- 🌐 **扩展拉丁字母查询** — 默认的 `LatinLanguages` 现在包含英语、法语和意大利语（`"en,fr,it"`）
- ⚙️ **Provider 注册** — 意大利语现已成为一级词典 provider

## 🆕 新功能（v1.4.0）

- 🇫🇷 **法语词典支持** — 通过 Collins 法英词典添加了法语支持，并以 Wiktionnaire 作为后备
- 🤖 **自动语言检测** — 可使用自然查询，如 `def world`、`def Enchanté`、`def слово`
- 🌐 **多语言拉丁字母查询** — 配置 `LatinLanguages` 设置（例如 `"en,fr"`）可同时查询多个拉丁字母词典
- ⚙️ **增强的配置** — 添加了 `LatinLanguages` 设置以灵活选择语言
- 🔄 **改进的 Provider 路由** — 更好地自动匹配多语言输入

## 🆕 新功能（v1.3.3）

- 🇺🇦 **乌克兰语词典** — 切换到 Wiktionary https://uk.wiktionary.org 作为主要数据源


## 📋 概览

Definition 是一个 [Microsoft PowerToys Run](https://github.com/microsoft/PowerToys) 插件，可让你无需离开键盘即可快速查询单词释义、音标和同义词。只需输入 `def <word>` 即可获取释义。该插件支持**英语**、**法语（Français）**、**意大利语（Italiano）**、**乌克兰语（Українська）**和**中文**，并具备自动文字检测功能——只需输入任何支持语言的单词，插件就会自动优先返回相应结果。

<div align="center">
  <img src="../data/demo-definition-2.gif" alt="Lookup word definitions" width="650">
</div>

## ✨ 功能特性

- 🔍 **即时释义**：通过 `dictionaryapi.dev` 实时获取释义。
- 🇫🇷 **法语词典（Français）**：通过 Collins 查询法语单词，以 Wiktionnaire 作为后备。
- 🇮🇹 **意大利语词典（Italiano）**：通过 Wikizionario 查询意大利语单词。
- 🇺🇦 **乌克兰语词典（Українська）**：使用 Wiktionary https://uk.wiktionary.org 作为主要数据源查询乌克兰语单词。
- 🇨🇳 **中文词典（中文）**：基于内置 CC-CEDICT 数据库（约 124,000 条词条）的离线中英查询——无需联网。
- 🔄 **多语言并行查询**：所有已配置的 provider 会同时被查询；结果会根据你输入的文字（拉丁字母、西里尔字母或汉字）进行优先级排序。
- 🤖 **自动语言检测**：可使用自然输入，如 `def world`、`def Enchanté` 或 `def слово`。
- 🔊 **发音音频**：直接在结果中播放音标音频。
- 📚 **音标与同义词**：查看音标拼写、同义词和反义词。
- 📝 **用法示例**：查看单词在实际中的使用示例。
- ⚙️ **完全可配置**：基于 JSON 的配置，包含 15+ 项可自定义设置。
- ⏱️ **延迟执行**：在获取结果前显示加载指示器。
- 💾 **智能缓存**：内存缓存用于重复查询，可配置缓存大小和过期时间。
- 🔄 **稳健的网络处理**：指数退避重试逻辑，确保 API 调用可靠。
- 🌓 **主题感知**：自动切换浅色/深色模式图标。
- 📋 **丰富的上下文菜单**：复制释义、播放发音、打开来源 URL 或搜索相关单词。
- 🔄 **可取消的请求**：输入新查询时自动取消之前的请求。
- 🌐 **Wiktionary 集成**：在 Wiktionary 中打开任何单词以获取更多信息和翻译。

## 🎬 演示

<div align="center">
  <img src="../data/demo-definition.gif" alt="Definition Plugin Demo" width="650">
</div>

## 🚀 安装

### 前提条件

- 已安装 [PowerToys Run](https://github.com/microsoft/PowerToys/releases)（v0.70.0 或更高版本）
- Windows 10（内部版本 22621）或更高版本
- .NET 9.0 运行时（随 Windows 11 22H2 或更高版本提供）
- 网络连接（用于 API 访问）

### 快速安装（手动）

1. 根据你的系统架构下载相应的 ZIP 文件：
   - [x64 版本](https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-x64.zip)
   - [ARM64 版本](https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-ARM64.zip)

2. 将 ZIP 解压到：
   ```
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```
   
   典型路径：`C:\Users\你的用户名\AppData\Local\Microsoft\PowerToys\PowerToys Run\Plugins\`

3. 重启 PowerToys（右键点击系统托盘中的 PowerToys 图标，选择"重启"）。

4. 打开 PowerToys Run（`Alt + Space`）并输入 `def <word>`。

### 手动验证

要验证插件是否正确安装：

1. 打开 PowerToys 设置
2. 导航到 PowerToys Run > Plugins
3. 在插件列表中查找"Definition"
4. 确保它已启用（开关应为开启状态）

## 🔧 使用方法

1. 激活 PowerToys Run（`Alt + Space`）。
2. 输入：
   - `def` 查看说明。
   - `def <word>` 根据语言/文字自动查询释义。
   - **子命令**（v1.5.1+）：
     - `def pronunciation <word>` / `def pron <word>` — 仅显示发音 + 音频
     - `def synonyms <word>` / `def syn <word>` — 仅显示同义词
     - `def antonyms <word>` / `def ant <word>` — 仅显示反义词
     - `def examples <word>` / `def ex <word>` — 仅显示用法示例
3. 按 <kbd>Enter</kbd> 获取结果。
4. 使用 <kbd>Ctrl + C</kbd> 复制释义。
5. 右键点击结果可以：
   - 使用 <kbd>Ctrl + C</kbd> 复制释义
   - 播放发音音频
   - 在 Wiktionary 中打开该单词
   - 搜索相关单词

<div align="center">
  <img src="../data/demo-subcommands.gif" alt="Subcommand Demo" width="650">
</div>

## ⚙️ 配置

该插件支持通过 `config.json` 文件进行广泛的自定义，该文件会在插件目录中自动创建。更改立即生效，无需重启。

### 可用设置

| 设置 | 默认值 | 说明 |
|---------|---------|-------------|
| `Language` | `"en"` | 默认语言（`"en"`、`"fr"`、`"it"`、`"uk"` 或 `"zh"`） |
| `ApiEndpoint` | `https://api.dictionaryapi.dev/api/v2/entries/en/` | 英语词典 API 端点 |
| `LatinLanguages` | `"en,fr,it"` | 以逗号分隔的拉丁字母查询语言（例如 `"en,fr,it"` 表示英语、法语和意大利语） |
| `UkrainianApiEndpoint` | `https://sum.in.ua/s/` | 乌克兰语词典后备端点（sum.in.ua） |
| `ChineseApiEndpoint` | `https://www.mdbg.net/chinese/dictionary?...` | 中文词典参考 URL |
| `CacheMaxSize` | 100 | 缓存单词查询的最大数量 |
| `HttpTimeoutSeconds` | 10 | API 请求超时时间（秒） |
| `CacheExpirationMinutes` | 30 | 缓存条目的保留时间 |
| `EnableAudioPlayback` | true | 启用/禁用发音音频 |
| `EnableClipboardOperations` | true | 启用/禁用复制到剪贴板 |
| `TextTruncateLength` | 30 | 上下文菜单中文本的最大长度 |
| `EnableVerboseLogging` | false | 启用详细调试日志 |
| `MaxResultsPerMeaning` | 3 | 每个词义的最大释义数 |
| `ShowExamplesInResults` | true | 显示用法示例 |
| `ShowSynonymsInResults` | true | 显示同义词 |
| `ShowAntonymsInResults` | true | 显示反义词 |

### 配置示例

```json
{
  "Language": "en",
  "CacheMaxSize": 200,
  "HttpTimeoutSeconds": 15,
  "EnableAudioPlayback": true,
  "ShowSynonymsInResults": false,
  "ShowAntonymsInResults": false,
  "ShowExamplesInResults": true,
  "MaxResultsPerMeaning": 2,
  "EnableVerboseLogging": true
}
```

> **注意：** 你无需更改 `Language` 即可使用乌克兰语或中文。插件会自动检测你查询的文字。西里尔字母输入（例如 `def слово`）将优先显示乌克兰语结果，汉字将优先显示中文结果，拉丁字母输入将查询 `LatinLanguages` 中列出的语言。
>
> **多语言拉丁字母查询：** 设置 `"LatinLanguages": "en,fr,it"` 可同时查询英语、法语和意大利语词典。

## 📁 数据存储

所有设置都存储在标准的 PowerToys 设置文件中（不会创建额外的数据文件）。

## 🛠️ 从源码构建

```bash
git clone https://github.com/ruslanlap/PowerToysRun-Definition.git
cd PowerToysRun-Definition/Definition
dotnet build
# 打包：
dotnet publish -c Release -r win-x64 --output ./publish
zip -r Definition-v1.5.1-x64.zip ./publish
```

## 📊 项目结构

```
PowerToysRun-Definition/
├── data/                            # 插件资源（图标、演示）
│   ├── definition.dark.png
│   ├── definition.logo.png
│   ├── demo-definition.gif
│   └── demo-definition-2.gif
├── Definition/                      # 插件源码
│   ├── Community.PowerToys.Run.Plugin.Definition/
│   │   ├── Images/
│   │   │   ├── definition.dark.png
│   │   │   └── definition.light.png
│   │   ├── Main.cs
│   │   └── plugin.json
│   └── Community.PowerToys.Run.Plugin.Definition.csproj
└── README.md
```

## 🤝 参与贡献

欢迎提交贡献！以下是你可以提供帮助的方式：

1. Fork 该仓库
2. 创建功能分支：`git checkout -b feature/amazing-feature`
3. 提交更改：`git commit -m 'Add amazing feature'`
4. 推送到分支：`git push origin feature/amazing-feature`
5. 发起 Pull Request

请确保适当地更新测试。

### 贡献者

- [ruslanlap](https://github.com/ruslanlap) - 项目创建者和维护者

## ❓ 常见问题

<details>
<summary><b>该插件需要联网吗？</b></summary>
<p>英语、法语、意大利语和乌克兰语查询需要联网（分别为 dictionaryapi.dev、collinsdictionary.com/wiktionary、it.wiktionary.org 和 uk.wiktionary.org）。中文查询使用内置的离线词典，无需联网。所有结果都会缓存在内存中，以便后续查询。</p>
</details>

<details>
<summary><b>如何更改插件的主题？</b></summary>
<p>插件会自动适配你的 PowerToys 主题（浅色/深色）。图标会根据你当前的系统主题动态加载。</p>
</details>

<details>
<summary><b>释义会被缓存吗？</b></summary>
<p>是的，释义会在当前会话期间缓存在内存中（最多 100 条），以提高性能并减少 API 调用。</p>
</details>

<details>
<summary><b>我可以自定义词典数据源吗？</b></summary>
<p>可以。你可以在 <code>config.json</code> 中更改 <code>ApiEndpoint</code>（英语）和 <code>UkrainianApiEndpoint</code>（乌克兰语）。中文查询使用内置的 CC-CEDICT 数据库。</p>
</details>

<details>
<summary><b>如何查询乌克兰语单词？</b></summary>
<p>只需输入 <code>def слово</code>（任何西里尔字母的乌克兰语单词）。插件会自动检测西里尔字母并优先显示乌克兰语结果。主要数据源是 <a href="https://goroh.pp.ua/">goroh.pp.ua</a>（Горох — українські словники，500,000+ 单词），以 <a href="https://sum.in.ua/">sum.in.ua</a> 作为后备。无需特殊的 API 密钥。</p>
</details>

<details>
<summary><b>支持哪些语言？</b></summary>
<p>开箱即用支持五种语言：</p>
<ul>
<li><strong>English</strong> — 通过 <a href="https://dictionaryapi.dev/">dictionaryapi.dev</a>（免费 REST API）</li>
<li><strong>French（Français）</strong> — 通过 <a href="https://www.collinsdictionary.com/dictionary/french-english/">Collins French-English Dictionary</a>（主要）+ <a href="https://fr.wiktionary.org/">Wiktionnaire</a>（后备）</li>
<li><strong>Italian（Italiano）</strong> — 通过 <a href="https://it.wiktionary.org/">Wikizionario</a></li>
<li><strong>Ukrainian（Українська）</strong> — 通过 <a href="https://uk.wiktionary.org/">Wiktionary</a>（主要）+ <a href="https://goroh.pp.ua/">goroh.pp.ua</a>（后备）</li>
<li><strong>Chinese（中文）</strong> — 通过内置 CC-CEDICT 数据库（约 124,000 词条，完全离线）</li>
</ul>
</details>

<details>
<summary><b>为什么插件在显示结果前会显示"正在查询..."？</b></summary>
<p>该插件实现了 IDelayedExecutionPlugin，在从 API 获取结果时会显示加载指示器。这可以在请求处理时提供即时反馈。</p>
</details>

<details>
<summary><b>如何播放发音音频？</b></summary>
<p>右键点击任何释义结果，从上下文菜单中选择"播放发音"（仅当 API 为该单词提供音频时可用）。</p>
</details>

<details>
<summary><b>如何查看某个单词的更多信息？</b></summary>
<p>右键点击任何结果，选择"在浏览器中打开来源 URL"，即可在 Wiktionary 中查看该单词，获取更多释义、翻译和词源信息。</p>
</details>

<details>
<summary><b>WinGet 和手动安装有什么区别？</b></summary>
<p><strong>WinGet 安装：</strong>运行一条命令（<code>winget install ruslanlap.DefinitionForCommandPalette</code>），WinGet 会处理一切——自动下载、验证、安装并注册扩展。当新版本发布时，你还会收到自动更新通知。</p>
<p><strong>手动安装：</strong>下载 ZIP 文件，解压到特定文件夹，重启 PowerToys。你需要在 GitHub 上手动检查更新。</p>
<p>对大多数用户推荐使用 WinGet，因为它更方便，并确保你始终拥有最新版本。</p>
</details>

## 🔆 功能亮点

本节重点介绍 Definition 插件的一些最强大的功能：

<div align="center">
  <figure>
    <img src="../data/demo8.png" width="800" alt="Wiktionary Integration">
    <figcaption>
      <strong>Wiktionary 集成</strong> - 通过上下文菜单直接在 Wiktionary 中打开任何单词，访问全面的单词信息。获取更多释义、翻译、词源和相关词条。
    </figcaption>
  </figure>
  
  <figure>
    <img src="../data/demo9.png" width="800" alt="Advanced Context Menu">
    <figcaption><strong>丰富的上下文菜单</strong> - 该插件提供了功能强大的上下文菜单，包含多种操作。
      复制释义、播放发音音频、打开来源 URL，以及搜索相关单词。
      右键点击任何结果即可访问这些功能。
    </figcaption>
  </figure>
</div>

## 🧑‍💻 技术栈

| 技术 | 说明 |
|---|---|
| C# / .NET 9.0 | 主要语言和运行时 |
| PowerToys Run API | IPlugin、IDelayedExecutionPlugin、IContextMenu 接口 |
| HttpClient | 带超时处理的 API 请求 |
| System.Text.Json | JSON 解析 |
| WPF MediaPlayer | 音频播放 |
| System.Threading | 异步操作 |
| GitHub Actions | 支持多架构构建的 CI/CD |

## 🌐 支持语言

该插件支持四个词典数据源，并具备自动文字检测：

| 语言 | 数据源 | 方式 | 需要联网 |
|----------|--------|--------|:-----------------:|
| **English** | [dictionaryapi.dev](https://dictionaryapi.dev/) | REST API（JSON） | 是 |
| **Français** | [Collins](https://www.collinsdictionary.com/dictionary/french-english/)（主要）+ [Wiktionnaire](https://fr.wiktionary.org/)（后备） | HTML 解析 + MediaWiki API | 是 |
| **Українська** | [Wiktionary](https://uk.wiktionary.org/)（主要）+ [goroh.pp.ua](https://goroh.pp.ua/)（后备） | API + HTML 抓取 | 是 |
| **中文** | CC-CEDICT（内置，约 124,000 词条） | 离线数据库 | 否 |

**工作原理：** 当你输入 `def <word>` 时，插件会检测你输入的文字并查询相应的 provider：
- 西里尔字母输入（`def слово`）→ 优先显示乌克兰语结果
- 汉字（`def 你好`）→ 优先显示中文结果
- 拉丁字母输入（`def hello` / `def enchanté`）→ 查询 `LatinLanguages` 配置中的语言（默认：英语 + 法语）

> **关于乌克兰语的说明：** 乌克兰语词典没有公开的 REST API。该插件使用 [goroh.pp.ua](https://goroh.pp.ua/)（Горох — українські словники）作为主要数据源——这是一个综合性的乌克兰语词典，包含 500,000+ 单词、释义、示例、同义词等。西里尔字母单词直接用于 URL 中（例如 `def слово` → `https://goroh.pp.ua/Тлумачення/слово`）。如果 goroh.pp.ua 不可用，则使用 [sum.in.ua](https://sum.in.ua/) 作为后备。

## 📸 截图

<div style="display:flex;flex-wrap:wrap;justify-content:center;gap:20px;">
  <figure style="margin:0;">
    <img src="../data/demo1.png" width="300" alt="Word Definition">
    <figcaption style="text-align:center;">单词释义</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo2.png" width="300" alt="Phonetics Display">
    <figcaption style="text-align:center;">音标显示</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo3.png" width="300" alt="Context Menu">
    <figcaption style="text-align:center;">上下文菜单</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo4.png" width="300" alt="Antonyms Feature">
    <figcaption style="text-align:center;">反义词功能</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo5.png" width="300" alt="Audio Pronunciation">
    <figcaption style="text-align:center;">音频发音</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo6.png" width="300" alt="Delayed Execution">
    <figcaption style="text-align:center;">延迟执行</figcaption>
  </figure>
</div>

## 📄 许可证

本项目基于 MIT 许可证授权——详情请参阅 [LICENSE](LICENSE) 文件。

## 🙏 致谢

- [Microsoft PowerToys](https://github.com/microsoft/PowerToys) 团队提供了出色的启动器
- [dictionaryapi.dev](https://dictionaryapi.dev/) 提供了免费的英语词典 API
- [Collins Dictionary](https://www.collinsdictionary.com/dictionary/french-english/) 提供了法英词典内容
- [Wiktionnaire](https://fr.wiktionary.org/) 提供了法语后备释义
- [goroh.pp.ua](https://goroh.pp.ua/) 提供了 Горох — українські словники（主要乌克兰语词典数据源）需要 API，请写信给 goroh.pp.ua 的开发者以便将 API 添加到插件中。
- [sum.in.ua](https://sum.in.ua/) 提供了 Словник української мови（乌克兰语词典后备）目前不可用。
- [MDBG.net](https://www.mdbg.net) 提供了 CC-CEDICT 中英词典的访问
- [Wiktionary](https://en.wiktionary.org/) 提供了全面的单词信息和翻译
- 所有帮助改进此插件的贡献者

## ☕ 支持

如果你觉得此插件有用并希望支持其开发，可以请我喝杯咖啡：

[![Buy me a coffee](https://img.shields.io/badge/Buy%20me%20a%20coffee-☕️-FFDD00?style=for-the-badge&logo=buy-me-a-coffee)](https://ruslanlap.github.io/ruslanlap_buymeacoffe/)

## 🆕 新功能（v1.2.2）

- 🇺🇦 **乌克兰语词典支持** — 集成了 `sum.in.ua` 释义词典。目前不可用。
- 🇨🇳 **中文词典支持** — 集成了 `MDBG.net`（CC-CEDICT 数据）用于中英查询。
- 🔄 **并行查询** — 同时从英语、乌克兰语和中文数据源获取结果。
- 🎯 **智能优先级** — 根据查询文字（西里尔字母、汉字或拉丁字母）自动对结果进行优先级排序。
- 🏗️ **改进的架构** — 重构为基于 provider 的系统，以获得更好的可扩展性。
- 🩹 **更好的可靠性** — 增强的错误处理确保一个 provider 失败不会破坏整个搜索。
- 📦 **依赖项** — 添加了 `HtmlAgilityPack` 用于稳健地解析乌克兰语和中文结果的 HTML。

## 🆕 新功能（v1.2.1）

- ⚙️ **完全可配置的设置** — 基于 JSON 的配置系统，支持运行时更新：
  - `config.json` 包含 11 项可自定义设置
  - 切换同义词、反义词、示例的显示
  - 配置缓存大小、超时和结果限制
  - 启用/禁用音频播放和剪贴板操作
  - 设置自动重新加载，无需重启
- 🔄 **稳健的网络重试逻辑** — 增强了 API 调用的可靠性：
  - 带智能重试条件的指数退避
  - 优雅地处理瞬时网络错误
  - 可配置的重试次数和延迟
- 🛠️ **改进的剪贴板操作** — 更好的线程处理和可靠性：
  - 用于线程安全的自定义 STA 任务调度器
  - 增强的错误处理和超时保护
  - 可配置的剪贴板操作启用/禁用
- 🔧 **配置错误修复** — 设置现在真正生效：
  - 修复了 config.json 更改被忽略的问题
  - 所有配置选项现在都被正确遵循
  - 动态重新加载确保立即生效
- 📊 **增强的调试** — 更好的故障排除能力：
  - 用于详细诊断的详细日志选项
  - 改进了整个插件的错误报告
  - 更好的网络错误分类

---

<div align="center">
  <sub>Made with ❤️ by <a href="https://github.com/ruslanlap">ruslanlap</a></sub>
</div>
