# 🔍 PowerToys Run: Definition Plugin (Українська)

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

`Definition` — це плагін для PowerToys Run, який дозволяє отримувати визначення слів, транскрипцію, приклади, синоніми та антоніми прямо з пошуку за командою `def <слово>`.

## 📋 Зміст

- [Огляд](#-огляд)
- [Ключові можливості](#-ключові-можливості)
- [Підтримувані мови](#-підтримувані-мови)
- [Встановлення](#-встановлення)
- [Використання](#-використання)
- [Налаштування](#️-налаштування)
- [Швидка діагностика](#-швидка-діагностика)
- [Посилання](#-посилання)

## 📌 Огляд

Плагін оптимізований для швидких запитів у стилі:

- `def world`
- `def enchanté`
- `def amore`
- `def слово`
- `def 你好`

Мова визначається автоматично за скриптом введення:

- латиниця → англійська/французька/італійська (за `LatinLanguages`)
- кирилиця → українська
- китайські ієрогліфи → китайська

## ✨ Ключові можливості

- Автоматичне визначення мови без префіксів (`def <слово>`)
- Паралельний пошук у кількох словникових провайдерах
- Відтворення аудіо вимови (якщо джерело надає аудіо)
- Копіювання визначення через контекстне меню
- Гнучка конфігурація через `config.json`
- Кешування результатів для прискорення повторних запитів
- Підтримка офлайн-словника для китайської мови

## 🌐 Підтримувані мови

| Мова | Джерело | Метод | Інтернет |
|---|---|---|:---:|
| **English** | [dictionaryapi.dev](https://dictionaryapi.dev/) | REST API (JSON) | Так |
| **Français** | [Collins](https://www.collinsdictionary.com/dictionary/french-english/) (основне) + [Wiktionnaire](https://fr.wiktionary.org/) (резерв) | HTML + MediaWiki API | Так |
| **Italiano** | [Wikizionario](https://it.wiktionary.org/) | MediaWiki API | Так |
| **Українська** | [uk.wiktionary.org](https://uk.wiktionary.org/) (основне) + [goroh.pp.ua](https://goroh.pp.ua/) (резерв) | MediaWiki API + HTML | Так |
| **中文** | Вбудована база CC-CEDICT | Офлайн-база | Ні |

## 🚀 Встановлення


### Ручне встановлення

1. Завантажте архів з [останнього релізу](https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest).
2. Розпакуйте файли в директорію:

   ```text
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```

3. Перезапустіть PowerToys.
4. Відкрийте PowerToys Run (`Alt + Space`) і виконайте `def test`.

## 🔧 Використання

1. Відкрийте PowerToys Run (`Alt + Space`).
2. Введіть `def <слово>`.
3. Натисніть <kbd>Enter</kbd>.
4. У контекстному меню результату доступно:
   - копіювання визначення
   - відтворення вимови
   - відкриття джерела (Wiktionary/словник)
   - пошук пов’язаних слів

### Приклади запитів

- `def world`
- `def enchanté`
- `def amore`
- `def слово`
- `def 你好`

## ⚙️ Налаштування

Плагін автоматично створює `config.json` у директорії плагіна.

### Основні параметри

| Параметр | Значення за замовчуванням | Опис |
|---|---|---|
| `Language` | `"en"` | Мова за замовчуванням |
| `LatinLanguages` | `"en,fr,it"` | Латинські мови для паралельного пошуку |
| `ApiEndpoint` | `https://api.dictionaryapi.dev/api/v2/entries/en/` | API для англійської |
| `HttpTimeoutSeconds` | `30` | Таймаут HTTP-запитів |
| `CacheMaxSize` | `100` | Максимум записів у кеші |
| `CacheExpirationMinutes` | `30` | Термін життя кешу |
| `EnableAudioPlayback` | `true` | Увімкнути аудіо вимову |
| `EnableClipboardOperations` | `true` | Увімкнути копіювання |
| `MaxResultsPerMeaning` | `3` | Ліміт визначень на значення |
| `ShowExamplesInResults` | `true` | Показувати приклади |
| `ShowSynonymsInResults` | `true` | Показувати синоніми |
| `ShowAntonymsInResults` | `true` | Показувати антоніми |
| `EnableVerboseLogging` | `false` | Детальне логування |

### Приклад конфігурації

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

## 🧪 Швидка діагностика

Якщо для слова немає результатів:

1. Перевірте інтернет-з'єднання (для `en`, `fr`, `it`, `uk`).
2. Спробуйте варіант без діакритики (наприклад, `enchante` замість `enchanté`).
3. Перевірте, чи плагін активний у `PowerToys Run > Plugins`.
4. Перезапустіть PowerToys після оновлення плагіна.

## 🔗 Посилання

- Повна англомовна документація: [../README.md](../README.md)
- Релізи: [GitHub Releases](https://github.com/ruslanlap/PowerToysRun-Definition/releases)
- Проєкт: [PowerToysRun-Definition](https://github.com/ruslanlap/PowerToysRun-Definition)
