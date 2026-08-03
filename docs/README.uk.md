# 🔍 PowerToys Run: Плагін Definition

<div align="center">
  <img src="../data/definition.logo.png" alt="Логотип плагіна Definition" width="128" height="128">
</div>

<div align="center">
  <h1>Definition</h1>
  <p>Пошук визначень слів, фонетики та синонімів безпосередньо у PowerToys Run.</p>
  <img src="../data/demo-definition.gif" alt="Демонстрація плагіна Definition" width="650">
</div>



<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml">
    <img src="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml/badge.svg" alt="Статус збірки">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/v/release/ruslanlap/PowerToysRun-Definition?label=latest" alt="Останній реліз">
  </a>
  <img src="https://img.shields.io/badge/version-v1.5.1-brightgreen" alt="Версія">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/stargazers">
    <img src="https://img.shields.io/github/stars/ruslanlap/PowerToysRun-Definition" alt="GitHub зірки">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/issues">
    <img src="https://img.shields.io/github/issues/ruslanlap/PowerToysRun-Definition" alt="GitHub задачі">
  </a>
  <a href="https://opensource.org/licenses/MIT">
    <img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="Ліцензія">
      <img src="https://img.shields.io/badge/Made%20with-❤️-red" alt="Створено з любов'ю">
  <img src="https://img.shields.io/badge/Awesome-Yes-orange" alt="Чудовий">
          <a href="https://github.com/hlaueriksson/awesome-powertoys-run-plugins">
    <img src="https://awesome.re/mentioned-badge.svg" alt="Згадується в Awesome PowerToys Run Plugins">
  </a>
  <a href="https://winstall.app/apps/ruslanlap.DefinitionForCommandPalette">
    <img src="https://img.shields.io/badge/Install%20with-WinGet-blue.svg" alt="Встановити через WinGet">
  </a>
</div>

<div align="center">
  <a href="README.uk.md">
    <img src="https://img.shields.io/badge/docs-Українська-0057B7" alt="Документація Українська">
  </a>
  <a href="../README.md">
    <img src="https://img.shields.io/badge/docs-English-2EA44F" alt="Документація Англійська">
  </a>
  <a href="README.fr.md">
    <img src="https://img.shields.io/badge/docs-Français-1F6FEB" alt="Документація Французька">
  </a>
  <a href="README.it.md">
    <img src="https://img.shields.io/badge/docs-Italiano-009246" alt="Документація Італійська">
  </a>
  <a href="README.zh.md">
    <img src="https://img.shields.io/badge/docs-中文-E34C26" alt="Документація Китайська">
  </a>
</div>

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-x64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-x64-blue?style=for-the-badge&logo=github" alt="Завантажити x64">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-ARM64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-ARM64-blue?style=for-the-badge&logo=github" alt="Завантажити ARM64">
  </a>
    <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/downloads/ruslanlap/PowerToysRun-Definition/total?style=for-the-badge&logo=github" alt="Усі релізи GitHub">
  </a>
</div>

## 📋 Зміст

- [📋 Огляд](#-огляд)
- [✨ Можливості](#-можливості)
- [🎬 Демонстрація](#-демонстрація)
- [🚀 Встановлення](#-встановлення)
- [🔧 Використання](#-використання)
- [⚙️ Конфігурація](#️-конфігурація)
- [📁 Зберігання даних](#-зберігання-даних)
- [🛠️ Збірка з вихідного коду](#️-збірка-з-вихідного-кодів)
- [📊 Структура проєкту](#-структура-прожкту)
- [🤝 Участь у проєкті](#-участь-у-прожкті)
- [❓ Поширені запитання](#-поширені-запитання)
- [🧑‍💻 Технологічний стек](#-технологічний-стек)
- [🌐 Локалізація](#-локалізація)
- [📸 Знімки екрана](#-знімки-екрана)
- [📄 Ліцензія](#-ліцензія)
- [🙏 Подяки](#-подяки)
- [☕ Підтримка](#-підтримка)
- [🆕 Що нового (v1.5.1)](#-що-нового-v150)
- [🆕 Що нового (v1.4.0)](#-що-нового-v140)
- [🆕 Що нового (v1.3.3)](#-що-нового-v133)
- [🆕 Що нового (v1.3.2)](#-що-нового-v132)
- [🆕 Що нового (v1.3.1)](#-що-нового-v131)

## 🆕 Що нового (v1.5.1)

- ⌨️ **Підтримка підкоманд** — Швидкий доступ до конкретних даних слова без повного пошуку визначень:
  - `def pronunciation <word>` / `def pron <word>` — Показати лише вимову та аудіо
  - `def synonyms <word>` / `def syn <word>` — Показати лише синоніми
  - `def antonyms <word>` / `def ant <word>` — Показати лише антоніми
  - `def examples <word>` / `def ex <word>` — Показати лише приклади вживання
  - За замовчуванням: `def <word>` показує все (визначення + фонетика + синоніми + антоніми + приклади)
- Працює для всіх підтримуваних мов (англійська, французька, італійська, українська, китайська)
- Налаштовується через існуючі параметри `ShowSynonymsInResults`, `ShowAntonymsInResults`, `ShowExamplesInResults`

## 🆕 Що нового (v1.5.1)

- 🇮🇹 **Підтримка італійського словника** — Додано пошук італійських слів через Wikizionario (`it.wiktionary.org`)
- 🌐 **Розширений пошук для латинки** — За замовчуванням `LatinLanguages` тепер включає англійську, французьку та італійську (`"en,fr,it"`)
- ⚙️ **Реєстрація провайдерів** — Італійська тепер доступна як повноцінний постачальник словника

## 🆕 Що нового (v1.4.0)

- 🇫🇷 **Підтримка французького словника** — Додано французьку через словник Collins French-English із резервним Wiktionnaire
- 🤖 **Автоматичне визначення мови** — Використовуйте природні запити на кшталт `def world`, `def Enchanté`, `def слово`
- 🌐 **Багатомовний пошук для латинки** — Налаштуйте параметр `LatinLanguages` (наприклад, `"en,fr"`) для одночасного запиту до кількох словників латинського письма
- ⚙️ **Розширена конфігурація** — Додано параметр `LatinLanguages` для гнучкого вибору мов
- 🔄 **Покращена маршрутизація провайдерів** — Краще автоматичне зіставлення для багатомовного вводу

## 🆕 Що нового (v1.3.3)

- 🇺🇦 **Український словник** — Перемкнено на Wiktionary https://uk.wiktionary.org як основне джерело


## 📋 Огляд

Definition — це плагін для [Microsoft PowerToys Run](https://github.com/microsoft/PowerToys), що дозволяє швидко шукати визначення слів, фонетику та синоніми, не відриваючись від клавіатури. Просто введіть `def <word>`, щоб отримати визначення. Плагін підтримує **англійську**, **французьку (Français)**, **італійську (Italiano)**, **українську (Українська)** та **китайську (中文)** з автоматичним визначенням письма — достатньо ввести слово будь-якою підтримуваною мовою, і плагін пріоритетно відсортує результати.

<div align="center">
  <img src="../data/demo-definition-2.gif" alt="Пошук визначень слів" width="650">
</div>

## ✨ Можливості

- 🔍 **Миттєві визначення**: Отримуйте визначення в реальному часі через `dictionaryapi.dev`.
- 🇫🇷 **Французький словник (Français)**: Пошук французьких слів через Collins із резервним Wiktionnaire.
- 🇮🇹 **Італійський словник (Italiano)**: Пошук італійських слів через Wikizionario.
- 🇺🇦 **Український словник (Українська)**: Пошук українських слів через Wiktionary https://uk.wiktionary.org як основне джерело.
- 🇨🇳 **Китайський словник (中文)**: Офлайн-пошук китайсько-англійських відповідностей на основі вбудованої бази CC-CEDICT (~124 000 записів) — без потреби в інтернеті.
- 🔄 **Паралельний багатомовний пошук**: Усі налаштовані провайдери опитуються одночасно; результати пріоритезуються залежно від письма запиту (латиниця, кирилиця або китайські ієрогліфи).
- 🤖 **Автоматичне визначення мови**: Використовуйте природний ввід, як-от `def world`, `def Enchanté` або `def слово`.
- 🔊 **Аудіо вимови**: Відтворюйте фонетичне аудіо безпосередньо з результатів.
- 📚 **Фонетика та синоніми**: Переглядайте фонетичний запис, синоніми та антоніми.
- 📝 **Приклади вживання**: Дивіться реальні приклади використання слів.
- ⚙️ **Повністю налаштовується**: Конфігурація на основі JSON із понад 15 параметрами.
- ⏱️ **Відкладене виконання**: Показує індикатор завантаження перед отриманням результатів.
- 💾 **Розумне кешування**: Кеш у пам'яті для повторних пошуків із налаштовуваним розміром і терміном дії.
- 🔄 **Надійна обробка мережі**: Логіка повторних спроб із експоненційною затримкою для стабільних API-викликів.
- 🌓 **Підтримка тем**: Автоматичне перемикання піктограм для світлої/темної теми.
- 📋 **Багате контекстне меню**: Копіювати визначення, відтворити вимову, відкрити URL-адресу джерела або шукати споріднені слова.
- 🔄 **Скасовувані запити**: Автоматично скасовує попередні запити під час введення нових.
- 🌐 **Інтеграція з Wiktionary**: Відкривайте будь-яке слово у Wiktionary для додаткової інформації та перекладів.

## 🎬 Демонстрація

<div align="center">
  <img src="../data/demo-definition.gif" alt="Демонстрація плагіна Definition" width="650">
</div>

## 🚀 Встановлення

### Попередні вимоги

- Встановлений [PowerToys Run](https://github.com/microsoft/PowerToys/releases) (v0.70.0 або пізніше)
- Windows 10 (збірка 22621) або пізніше
- .NET 9.0 Runtime (постачається з Windows 11 22H2 або пізніше)
- Підключення до інтернету (для доступу до API)

### Швидке встановлення (Вручну)

1. Завантажте відповідний ZIP-архів для архітектури вашої системи:
   - [Версія x64](https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-x64.zip)
   - [Версія ARM64](https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-ARM64.zip)

2. Розпакуйте ZIP до:
   ```
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```

   Типовий шлях: `C:\Users\YourUsername\AppData\Local\Microsoft\PowerToys\PowerToys Run\Plugins\`

3. Перезапустіть PowerToys (клацніть правою кнопкою миші на піктограмі PowerToys у системному треї та виберіть «Restart»).

4. Відкрийте PowerToys Run (`Alt + Space`) і введіть `def <word>`.

### Перевірка вручну

Щоб переконатися, що плагін коректно встановлено:

1. Відкрийте налаштування PowerToys
2. Перейдіть до PowerToys Run > Plugins
3. Знайдіть «Definition» у списку плагінів
4. Переконайтеся, що він увімкнений (перемикач має бути в положенні ON)

## 🔧 Використання

1. Активуйте PowerToys Run (`Alt + Space`).
2. Введіть:
   - `def`, щоб побачити інструкції.
   - `def <word>`, щоб шукати визначення автоматично на основі мови/письма.
   - **Підкоманди** (v1.5.1+):
     - `def pronunciation <word>` / `def pron <word>` — показати лише вимову + аудіо
     - `def synonyms <word>` / `def syn <word>` — показати лише синоніми
     - `def antonyms <word>` / `def ant <word>` — показати лише антоніми
     - `def examples <word>` / `def ex <word>` — показати лише приклади вживання
3. Натисніть <kbd>Enter</kbd>, щоб отримати результати.
4. Використовуйте <kbd>Ctrl + C</kbd>, щоб скопіювати визначення.
5. Клацніть правою кнопкою миші на результаті, щоб:
   - Скопіювати визначення через <kbd>Ctrl + C</kbd>
   - Відтворити аудіо вимови
   - Відкрити слово у Wiktionary
   - Шукати споріднені слова

<div align="center">
  <img src="../data/demo-subcommands.gif" alt="Демонстрація підкоманд" width="650">
</div>

## ⚙️ Конфігурація

Плагін підтримує розширене налаштування через файл `config.json`, який автоматично створюється в каталозі плагіна. Зміни набувають чинності негайно, без перезапуску.

### Доступні параметри

| Параметр | За замовчуванням | Опис |
|---------|---------|-------------|
| `Language` | `"en"` | Мова за замовчуванням (`"en"`, `"fr"`, `"it"`, `"uk"` або `"zh"`) |
| `ApiEndpoint` | `https://api.dictionaryapi.dev/api/v2/entries/en/` | Кінцева точка API англійського словника |
| `LatinLanguages` | `"en,fr,it"` | Мови латинського письма через кому для опитування (наприклад `"en,fr,it"` для англійської, французької та італійської) |
| `UkrainianApiEndpoint` | `https://sum.in.ua/s/` | Резервна кінцева точка українського словника (sum.in.ua) |
| `ChineseApiEndpoint` | `https://www.mdbg.net/chinese/dictionary?...` | Довідкова URL-адреса китайського словника |
| `CacheMaxSize` | 100 | Максимальна кількість кешованих пошуків слів |
| `HttpTimeoutSeconds` | 10 | Час очікування API-запитів у секундах |
| `CacheExpirationMinutes` | 30 | Тривалість зберігання записів у кеші |
| `EnableAudioPlayback` | true | Увімкнути/вимкнути аудіо вимови |
| `EnableClipboardOperations` | true | Увімкнути/вимкнути копіювання в буфер обміну |
| `TextTruncateLength` | 30 | Максимальна довжина тексту в контекстному меню |
| `EnableVerboseLogging` | false | Увімкнути детальне журналювання для налагодження |
| `MaxResultsPerMeaning` | 3 | Максимум визначень на одне значення слова |
| `ShowExamplesInResults` | true | Показувати приклади вживання |
| `ShowSynonymsInResults` | true | Показувати синоніми |
| `ShowAntonymsInResults` | true | Показувати антоніми |

### Приклад конфігурації

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

> **Примітка:** Не потрібно змінювати `Language`, щоб користуватися українською чи китайською. Плагін автоматично визначає письмо вашого запиту. Введення кирилицею (наприклад `def слово`) пріоритезує українські результати, китайські ієрогліфи — китайські результати, а латиниця опитає мови, зазначені в `LatinLanguages`.
>
> **Багатомовний пошук для латинки:** Встановіть `"LatinLanguages": "en,fr,it"`, щоб одночасно опитувати англійський, французький та італійський словники для слів латинським письмом.

## 📁 Зберігання даних

Усі налаштування зберігаються у стандартному файлі налаштувань PowerToys (додаткові файли даних не створюються).

## 🛠️ Збірка з вихідного коду

```bash
git clone https://github.com/ruslanlap/PowerToysRun-Definition.git
cd PowerToysRun-Definition/Definition
dotnet build
# Для пакування:
dotnet publish -c Release -r win-x64 --output ./publish
zip -r Definition-v1.5.1-x64.zip ./publish
```

## 📊 Структура проєкту

```
PowerToysRun-Definition/
├── data/                            # Ресурси плагіна (піктограми, демо)
│   ├── definition.dark.png
│   ├── definition.logo.png
│   ├── demo-definition.gif
│   └── demo-definition-2.gif
├── Definition/                      # Вихідний код плагіна
│   ├── Community.PowerToys.Run.Plugin.Definition/
│   │   ├── Images/
│   │   │   ├── definition.dark.png
│   │   │   └── definition.light.png
│   │   ├── Main.cs
│   │   └── plugin.json
│   └── Community.PowerToys.Run.Plugin.Definition.csproj
└── README.md
```

## 🤝 Участь у проєкті

Внески вітаються! Ось як ви можете допомогти:

1. Зробіть форк репозиторію
2. Створіть гілку для функції: `git checkout -b feature/amazing-feature`
3. Зробіть коміт змін: `git commit -m 'Add amazing feature'`
4. Надішліть до гілки: `git push origin feature/amazing-feature`
5. Відкрийте Pull Request

Будь ласка, оновлюйте тести за потреби.

### Учасники

- [ruslanlap](https://github.com/ruslanlap) — творець проєкту та супровідник

## ❓ Поширені запитання

<details>
<summary><b>Чи потрібен плагіну доступ до інтернету?</b></summary>
<p>Пошук англійською, французькою, італійською та українською потребує доступу до інтернету (dictionaryapi.dev, collinsdictionary.com/wiktionary, it.wiktionary.org та uk.wiktionary.org відповідно). Пошук китайською використовує вбудований офлайн-словник і працює без інтернету. Усі результати кешуються в пам'яті для подальших запитів.</p>
</details>

<details>
<summary><b>Як змінити тему плагіна?</b></summary>
<p>Плагін автоматично адаптується до вашої теми PowerToys (світла/темна). Піктограми динамічно завантажуються залежно від поточної системної теми.</p>
</details>

<details>
<summary><b>Чи кешуються визначення?</b></summary>
<p>Так, визначення кешуються в пам'яті під час поточної сесії (до 100 записів) для підвищення продуктивності та зменшення API-викликів.</p>
</details>

<details>
<summary><b>Чи можу я змінити джерело словника?</b></summary>
<p>Так. Ви можете змінити <code>ApiEndpoint</code> (англійська) та <code>UkrainianApiEndpoint</code> (українська) у <code>config.json</code>. Пошук китайською використовує вбудовану базу CC-CEDICT.</p>
</details>

<details>
<summary><b>Як шукати українські слова?</b></summary>
<p>Просто введіть <code>def слово</code> (будь-яке українське слово кирилицею). Плагін автоматично розпізнає кирилицю та пріоритезує українські результати. Основне джерело — <a href="https://goroh.pp.ua/">goroh.pp.ua</a> (Горох — українські словники, понад 500 000 слів) із резервним <a href="https://sum.in.ua/">sum.in.ua</a>. Спеціальний API-ключ не потрібен.</p>
</details>

<details>
<summary><b>Які мови підтримуються?</b></summary>
<p>Із коробки підтримуються п'ять мов:</p>
<ul>
<li><strong>Англійська</strong> — через <a href="https://dictionaryapi.dev/">dictionaryapi.dev</a> (безкоштовний REST API)</li>
<li><strong>Французька (Français)</strong> — через <a href="https://www.collinsdictionary.com/dictionary/french-english/">Collins French-English Dictionary</a> (основне) + <a href="https://fr.wiktionary.org/">Wiktionnaire</a> (резервне)</li>
<li><strong>Італійська (Italiano)</strong> — через <a href="https://it.wiktionary.org/">Wikizionario</a></li>
<li><strong>Українська (Українська)</strong> — через <a href="https://uk.wiktionary.org/">Wiktionary</a> (основне) + <a href="https://goroh.pp.ua/">goroh.pp.ua</a> (резервне)</li>
<li><strong>Китайська (中文)</strong> — через вбудовану базу CC-CEDICT (~124 000 записів, повністю офлайн)</li>
</ul>
</details>

<details>
<summary><b>Чому плагін показує «Looking up...» перед відображенням результатів?</b></summary>
<p>Плагін реалізує IDelayedExecutionPlugin, який показує індикатор завантаження під час отримання результатів з API. Це забезпечує негайний зворотний зв'язок під час обробки запиту.</p>
</details>

<details>
<summary><b>Як відтворити аудіо вимови?</b></summary>
<p>Клацніть правою кнопкою миші на будь-якому результаті визначення та виберіть «Play Pronunciation» з контекстного меню (доступно лише якщо API надає аудіо для цього слова).</p>
</details>

<details>
<summary><b>Як дізнатися більше про слово?</b></summary>
<p>Клацніть правою кнопкою миші на будь-якому результаті та виберіть «Open Source URL in Browser», щоб переглянути слово у Wiktionary, де доступні додаткова інформація, переклади та етимологія.</p>
</details>

<details>
<summary><b>У чому різниця між WinGet і ручним встановленням?</b></summary>
<p><strong>Встановлення через WinGet:</strong> Виконайте одну команду (<code>winget install ruslanlap.DefinitionForCommandPalette</code>), і WinGet зробить усе сам — завантажить, перевірить, встановить та зареєструє розширення автоматично. Ви також отримаєте автоматичні сповіщення про оновлення при виході нових версій.</p>
<p><strong>Ручне встановлення:</strong> Завантажте ZIP-файл, розпакуйте до вказаного каталогу, перезапустіть PowerToys. Перевіряти наявність оновлень доведеться вручну на GitHub.</p>
<p>WinGet рекомендований для більшості користувачів, оскільки це зручніше і гарантує актуальну версію.</p>
</details>

## 🔆 Підбірка можливостей

Цей розділ висвітлює деякі з найпотужніших функцій плагіна Definition:

<div align="center">
  <figure>
    <img src="../data/demo8.png" width="800" alt="Інтеграція з Wiktionary">
    <figcaption>
      <strong>Інтеграція з Wiktionary</strong> - Отримуйте вичерпну інформацію про слово, відкриваючи будь-яке слово у Wiktionary безпосередньо з контекстного меню. Доступ до додаткових значень, перекладів, етимологій та споріднених термінів.
    </figcaption>
  </figure>

  <figure>
    <img src="../data/demo9.png" width="800" alt="Розширене контекстне меню">
    <figcaption><strong>Багате контекстне меню</strong> - Плагін пропонує потужне контекстне меню з кількома діями.
      Копіюйте визначення, відтворюйте аудіо вимови, відкривайте URL-адреси джерел та шукайте споріднені слова.
      Клацніть правою кнопкою миші на будь-якому результаті, щоб скористатися цими функціями.
    </figcaption>
  </figure>
</div>

## 🧑‍💻 Технологічний стек

| Технологія | Опис |
|---|---|
| C# / .NET 9.0 | Основна мова та середовище виконання |
| PowerToys Run API | Інтерфейси IPlugin, IDelayedExecutionPlugin, IContextMenu |
| HttpClient | API-запити з обробкою тайм-аутів |
| System.Text.Json | Парсинг JSON |
| WPF MediaPlayer | Відтворення аудіо |
| System.Threading | Асинхронні операції |
| GitHub Actions | CI/CD зі збірками для кількох архітектур |

## 🌐 Підтримувані мови

Плагін підтримує чотири джерела словників з автоматичним визначенням письма:

| Мова | Джерело | Метод | Потрібен інтернет |
|----------|--------|--------|:-----------------:|
| **Англійська** | [dictionaryapi.dev](https://dictionaryapi.dev/) | REST API (JSON) | Так |
| **Français** | [Collins](https://www.collinsdictionary.com/dictionary/french-english/) (основне) + [Wiktionnaire](https://fr.wiktionary.org/) (резервне) | Парсинг HTML + MediaWiki API | Так |
| **Українська** | [Wiktionary](https://uk.wiktionary.org/) (основне) + [goroh.pp.ua](https://goroh.pp.ua/) (резервне) | API + HTML-скрейпінг | Так |
| **中文** | CC-CEDICT (вбудована, ~124 000 записів) | Офлайн-база | Ні |

**Як це працює:** Коли ви вводите `def <word>`, плагін визначає письмо вводу та опитує відповідних провайдерів:
- Введення кирилицею (`def слово`) → Пріоритет українських результатів
- Китайські ієрогліфи (`def 你好`) → Пріоритет китайських результатів
- Введення латиницею (`def hello` / `def enchanté`) → Опитування мов із конфігурації `LatinLanguages` (за замовчуванням: англійська + французька)

> **Примітка про українську:** Не існує публічного REST API для українських словників. Плагін використовує [goroh.pp.ua](https://goroh.pp.ua/) (Горох — українські словники) як основне джерело — вичерпний український словник із понад 500 000 слів, визначень, прикладів, синонімів тощо. Кириличні слова використовуються безпосередньо в URL (наприклад `def слово` → `https://goroh.pp.ua/Тлумачення/слово`). Якщо goroh.pp.ua недоступний, використовується [sum.in.ua](https://sum.in.ua/) як резервне джерело.

## 📸 Знімки екрана

<div style="display:flex;flex-wrap:wrap;justify-content:center;gap:20px;">
  <figure style="margin:0;">
    <img src="../data/demo1.png" width="300" alt="Визначення слова">
    <figcaption style="text-align:center;">Визначення слова</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo2.png" width="300" alt="Відображення фонетики">
    <figcaption style="text-align:center;">Відображення фонетики</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo3.png" width="300" alt="Контекстне меню">
    <figcaption style="text-align:center;">Контекстне меню</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo4.png" width="300" alt="Функція антонімів">
    <figcaption style="text-align:center;">Функція антонімів</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo5.png" width="300" alt="Аудіо вимови">
    <figcaption style="text-align:center;">Аудіо вимови</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo6.png" width="300" alt="Відкладене виконання">
    <figcaption style="text-align:center;">Відкладене виконання</figcaption>
  </figure>
</div>

## 📄 Ліцензія

Цей проєкт ліцензовано за ліцензією MIT — дивіться файл [LICENSE](LICENSE) для деталей.

## 🙏 Подяки

- Команда [Microsoft PowerToys](https://github.com/microsoft/PowerToys) за дивовижний засіб запуску
- [dictionaryapi.dev](https://dictionaryapi.dev/) за безкоштовний API англійського словника
- [Collins Dictionary](https://www.collinsdictionary.com/dictionary/french-english/) за вміст французько-англійського словника
- [Wiktionnaire](https://fr.wiktionary.org/) за резервні французькі визначення
- [goroh.pp.ua](https://goroh.pp.ua/) за Горох — українські словники (основне джерело українського словника). ПОТРІБЕН API — напишіть розробникам goroh.pp.ua, щоб додати API до плагіна.
- [sum.in.ua](https://sum.in.ua/) за Словник української мови (резервне джерело) НЕ ПРАЦЮЄ.
- [MDBG.net](https://www.mdbg.net) за доступ до китайсько-англійського словника CC-CEDICT
- [Wiktionary](https://en.wiktionary.org/) за вичерпну інформацію про слова та переклади
- Усі учасники, які допомогли покращити цей плагін

## ☕ Підтримка

Якщо плагін корисний для вас і ви хочете підтримати його розвиток, можете купити мені каву:

[![Buy me a coffee](https://img.shields.io/badge/Buy%20me%20a%20coffee-☕️-FFDD00?style=for-the-badge&logo=buy-me-a-coffee)](https://ruslanlap.github.io/ruslanlap_buymeacoffe/)

## 🆕 Що нового (v1.2.2)

- 🇺🇦 **Підтримка українського словника** — Інтегровано з тлумачним словником `sum.in.ua`. НЕ ПРАЦЮЄ.
- 🇨🇳 **Підтримка китайського словника** — Інтегровано з `MDBG.net` (дані CC-CEDICT) для пошуку китайсько-англійських відповідностей.
- 🔄 **Паралельний пошук** — Одночасне отримання результатів з англійських, українських та китайських джерел.
- 🎯 **Розумне пріоритезування** — Результати автоматично пріоритезуються на основі письма запиту (кирилиця, китайські ієрогліфи або латиниця).
- 🏗️ **Покращена архітектура** — Рефакторинг на основі системи провайдерів для кращої розширюваності.
- 🩹 **Краща надійність** — Покращена обробка помилок гарантує, що збій одного провайдера не зламає весь пошук.
- 📦 **Залежності** — Додано `HtmlAgilityPack` для надійного парсингу HTML українських та китайських результатів.

## 🆕 Що нового (v1.2.1)

- ⚙️ **Повністю налаштовувані параметри** — Система конфігурації на основі JSON з оновленням під час виконання:
  - `config.json` з 11 параметрами налаштування
  - Перемикання відображення синонімів, антонімів, прикладів
  - Налаштування розміру кешу, тайм-аутів та лімітів результатів
  - Увімкнення/вимкнення відтворення аудіо та операцій з буфером обміну
  - Налаштування перезавантажуються автоматично без перезапуску
- 🔄 **Надійна логіка повторних мережевих спроб** — Підвищена стабільність API-викликів:
  - Експоненційна затримка з розумними умовами повтору
  - Коректна обробка тимчасових мережевих помилок
  - Налаштовувані кількість спроб та затримки
- 🛠️ **Покращені операції з буфером обміну** — Краща потоковість та надійність:
  - Користувацький планувальник завдань STA для потокобезпеки
  - Розширена обробка помилок та захист від тайм-аутів
  - Можливість увімкнення/вимкнення операцій з буфером обміну
- 🔧 **Виправлення помилок конфігурації** — Налаштування тепер реально працюють:
  - Виправлено проблему, коли зміни config.json ігнорувалися
  - Усі параметри конфігурації тепер коректно враховуються
  - Динамічне перезавантаження забезпечує негайний ефект
- 📊 **Розширене налагодження** — Кращі можливості усунення несправностей:
  - Опція детального журналювання для ретельної діагностики
  - Покращене звітування про помилки по всьому плагіну
  - Краща категоризація мережевих помилок

---

<div align="center">
  <sub>Створено з ❤️ від <a href="https://github.com/ruslanlap">ruslanlap</a></sub>
</div>
