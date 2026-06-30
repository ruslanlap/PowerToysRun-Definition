# 🔍 PowerToys Run: Definition Plugin（中文）

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
  <a href="README.zh.md">
    <img src="https://img.shields.io/badge/docs-中文-E34C26" alt="Docs Chinese">
  </a>
</div>

`Definition` 是一个 PowerToys Run 插件，可通过 `def <单词>` 快速获取词义、音标、例句、同义词和反义词。

## 📋 目录

- [概览](#-概览)
- [核心功能](#-核心功能)
- [支持语言](#-支持语言)
- [安装](#-安装)
- [使用方法](#-使用方法)
- [配置](#️-配置)
- [快速排查](#-快速排查)
- [相关链接](#-相关链接)

## 📌 概览

插件支持自然输入，无需语言前缀：

- `def world`
- `def enchanté`
- `def amore`
- `def слово`
- `def 你好`

系统会根据输入脚本自动判断优先语言：

- 拉丁字母 → 英语/法语/意大利语（由 `LatinLanguages` 控制）
- 西里尔字母 → 乌克兰语
- 汉字 → 中文

## ✨ 核心功能

- 自动语言识别，统一使用 `def <单词>`
- 多词典并行查询，提高命中率
- 支持发音播放（取决于词源是否提供音频）
- 支持从右键菜单复制释义
- 通过 `config.json` 灵活配置行为
- 内置缓存，加速重复查询
- 中文使用内置 CC-CEDICT 词库，离线可用

## 🌐 支持语言

| 语言 | 数据源 | 方式 | 需要联网 |
|---|---|---|:---:|
| **English** | [dictionaryapi.dev](https://dictionaryapi.dev/) | REST API（JSON） | 是 |
| **Français** | [Collins](https://www.collinsdictionary.com/dictionary/french-english/)（主）+ [Wiktionnaire](https://fr.wiktionary.org/)（备） | HTML + MediaWiki API | 是 |
| **Italiano** | [Wikizionario](https://it.wiktionary.org/) | MediaWiki API | 是 |
| **Українська** | [uk.wiktionary.org](https://uk.wiktionary.org/)（主）+ [goroh.pp.ua](https://goroh.pp.ua/)（备） | MediaWiki API + HTML | 是 |
| **中文** | 内置 CC-CEDICT 数据库 | 离线数据库 | 否 |

## 🚀 安装

### 手动安装

1. 从 [最新发布页](https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest) 下载压缩包。
2. 解压到以下目录：

   ```text
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```

3. 重启 PowerToys。
4. 打开 PowerToys Run（`Alt + Space`），输入 `def test` 验证。

## 🔧 使用方法

1. 打开 PowerToys Run（`Alt + Space`）。
2. 输入 `def <单词>`。
3. 按 <kbd>Enter</kbd> 执行查询。
4. 右键结果可执行：
   - 复制释义
   - 播放发音
   - 打开词源页面
   - 搜索相关词

### 查询示例

- `def world`
- `def enchanté`
- `def amore`
- `def слово`
- `def 你好`

## ⚙️ 配置

插件会在安装目录自动生成 `config.json`。

### 常用配置项

| 配置项 | 默认值 | 说明 |
|---|---|---|
| `Language` | `"en"` | 默认语言 |
| `LatinLanguages` | `"en,fr,it"` | 拉丁字母并行查询语言 |
| `ApiEndpoint` | `https://api.dictionaryapi.dev/api/v2/entries/en/` | 英语 API 地址 |
| `HttpTimeoutSeconds` | `30` | HTTP 超时（秒） |
| `CacheMaxSize` | `100` | 最大缓存条目数 |
| `CacheExpirationMinutes` | `30` | 缓存过期时间（分钟） |
| `EnableAudioPlayback` | `true` | 是否启用发音播放 |
| `EnableClipboardOperations` | `true` | 是否启用复制功能 |
| `MaxResultsPerMeaning` | `3` | 每个词义显示的最大释义数 |
| `ShowExamplesInResults` | `true` | 是否显示例句 |
| `ShowSynonymsInResults` | `true` | 是否显示同义词 |
| `ShowAntonymsInResults` | `true` | 是否显示反义词 |
| `EnableVerboseLogging` | `false` | 是否开启详细日志 |

### 配置示例

```json
{
  "Language": "en",
  "LatinLanguages": "en,fr,it",
  "HttpTimeoutSeconds": 30,
  "CacheMaxSize": 200,
  "EnableAudioPlayback": true,
  "ShowSynonymsInResults": true,
  "ShowAntonymsInResults": true,
  "ShowExamplesInResults": true,
  "EnableVerboseLogging": false
}
```

## 🧪 快速排查

如果没有返回结果：

1. 检查网络连接（`en`、`fr`、`it`、`uk` 需要联网）。
2. 尝试去掉重音符号（例如 `enchante` 替代 `enchanté`）。
3. 在 `PowerToys Run > Plugins` 中确认插件已启用。
4. 更新后重启 PowerToys 再测试。

## 🔗 相关链接

- 英文完整文档：[../README.md](../README.md)
- 发布页：[GitHub Releases](https://github.com/ruslanlap/PowerToysRun-Definition/releases)
- 项目主页：[PowerToysRun-Definition](https://github.com/ruslanlap/PowerToysRun-Definition)
