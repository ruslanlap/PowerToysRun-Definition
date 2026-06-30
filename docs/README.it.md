# 🔍 PowerToys Run: Definition Plugin (Italiano)

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

`Definition` è un plugin per PowerToys Run che permette di cercare rapidamente definizioni, fonetica, esempi, sinonimi e contrari usando `def <parola>`.

## 📋 Indice

- [Panoramica](#-panoramica)
- [Funzionalità principali](#-funzionalità-principali)
- [Lingue supportate](#-lingue-supportate)
- [Installazione](#-installazione)
- [Utilizzo](#-utilizzo)
- [Configurazione](#️-configurazione)
- [Risoluzione rapida dei problemi](#-risoluzione-rapida-dei-problemi)
- [Link utili](#-link-utili)

## 📌 Panoramica

Il plugin supporta query naturali senza prefisso di lingua:

- `def world`
- `def enchanté`
- `def amore`
- `def слово`
- `def 你好`

La lingua viene rilevata automaticamente in base allo script usato:

- alfabeto latino → inglese/francese/italiano (secondo `LatinLanguages`)
- alfabeto cirillico → ucraino
- caratteri cinesi → cinese

## ✨ Funzionalità principali

- Rilevamento automatico della lingua con `def <parola>`
- Ricerca parallela su più provider di dizionari
- Riproduzione audio della pronuncia (se disponibile dalla fonte)
- Copia rapida delle definizioni dal menu contestuale
- Configurazione flessibile tramite `config.json`
- Cache dei risultati per velocizzare le ricerche ripetute
- Dizionario cinese integrato e offline

## 🌐 Lingue supportate

| Lingua | Fonte | Metodo | Internet |
|---|---|---|:---:|
| **English** | [dictionaryapi.dev](https://dictionaryapi.dev/) | REST API (JSON) | Sì |
| **Français** | [Collins](https://www.collinsdictionary.com/dictionary/french-english/) (principale) + [Wiktionnaire](https://fr.wiktionary.org/) (fallback) | HTML + MediaWiki API | Sì |
| **Italiano** | [Wikizionario](https://it.wiktionary.org/) | MediaWiki API | Sì |
| **Українська** | [uk.wiktionary.org](https://uk.wiktionary.org/) (principale) + [goroh.pp.ua](https://goroh.pp.ua/) (fallback) | MediaWiki API + HTML | Sì |
| **中文** | Database CC-CEDICT integrato | Database offline | No |

## 🚀 Installazione

### Installazione manuale

1. Scarica l'archivio dall'[ultima release](https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest).
2. Estrai i file in:

   ```text
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```

3. Riavvia PowerToys.
4. Apri PowerToys Run (`Alt + Space`) e prova `def test`.

## 🔧 Utilizzo

1. Apri PowerToys Run (`Alt + Space`).
2. Digita `def <parola>`.
3. Premi <kbd>Enter</kbd>.
4. Dal menu contestuale di ogni risultato puoi:
   - copiare la definizione
   - riprodurre la pronuncia
   - aprire la fonte
   - cercare parole correlate

### Esempi

- `def world`
- `def enchanté`
- `def amore`
- `def слово`
- `def 你好`

## ⚙️ Configurazione

Il plugin crea automaticamente `config.json` nella cartella del plugin.

### Impostazioni principali

| Impostazione | Valore predefinito | Descrizione |
|---|---|---|
| `Language` | `"en"` | Lingua predefinita |
| `LatinLanguages` | `"en,fr,it"` | Lingue latine interrogate in parallelo |
| `ApiEndpoint` | `https://api.dictionaryapi.dev/api/v2/entries/en/` | Endpoint API inglese |
| `HttpTimeoutSeconds` | `30` | Timeout HTTP in secondi |
| `CacheMaxSize` | `100` | Numero massimo di elementi in cache |
| `CacheExpirationMinutes` | `30` | Durata della cache |
| `EnableAudioPlayback` | `true` | Abilita audio pronuncia |
| `EnableClipboardOperations` | `true` | Abilita copia negli appunti |
| `MaxResultsPerMeaning` | `3` | Numero massimo di definizioni per significato |
| `ShowExamplesInResults` | `true` | Mostra esempi |
| `ShowSynonymsInResults` | `true` | Mostra sinonimi |
| `ShowAntonymsInResults` | `true` | Mostra contrari |
| `EnableVerboseLogging` | `false` | Abilita log dettagliati |

### Esempio di configurazione

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

## 🧪 Risoluzione rapida dei problemi

Se non ottieni risultati:

1. Controlla la connessione Internet (`en`, `fr`, `it`, `uk` richiedono rete).
2. Prova una variante senza accenti (`enchante` invece di `enchanté`).
3. Verifica che il plugin sia attivo in `PowerToys Run > Plugins`.
4. Riavvia PowerToys dopo l'aggiornamento del plugin.

## 🔗 Link utili

- Documentazione completa in inglese: [../README.md](../README.md)
- Release: [GitHub Releases](https://github.com/ruslanlap/PowerToysRun-Definition/releases)
- Progetto: [PowerToysRun-Definition](https://github.com/ruslanlap/PowerToysRun-Definition)
