# 🔍 PowerToys Run: Plugin Definition

<div align="center">
  <img src="../data/definition.logo.png" alt="Logo del Plugin Definition" width="128" height="128">
</div>

<div align="center">
  <h1>Definition</h1>
  <p>Cerca definizioni di parole, fonetica e sinonimi direttamente in PowerToys Run.</p>
  <img src="../data/demo-definition.gif" alt="Demo del Plugin Definition" width="650">
</div>



<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml">
    <img src="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml/badge.svg" alt="Stato della Build">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/v/release/ruslanlap/PowerToysRun-Definition?label=latest" alt="Ultima Release">
  </a>
  <img src="https://img.shields.io/badge/version-v1.5.1-brightgreen" alt="Versione">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/stargazers">
    <img src="https://img.shields.io/github/stars/ruslanlap/PowerToysRun-Definition" alt="Stelle GitHub">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/issues">
    <img src="https://img.shields.io/github/issues/ruslanlap/PowerToysRun-Definition" alt="Issue GitHub">
  </a>
  <a href="https://opensource.org/licenses/MIT">
    <img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="Licenza">
      <img src="https://img.shields.io/badge/Made%20with-❤️-red" alt="Fatto con Amore">
  <img src="https://img.shields.io/badge/Awesome-Yes-orange" alt="Awesome">
          <a href="https://github.com/hlaueriksson/awesome-powertoys-run-plugins">
    <img src="https://awesome.re/mentioned-badge.svg" alt="Segnalato in Awesome PowerToys Run Plugins">
  </a>
  <a href="https://winstall.app/apps/ruslanlap.DefinitionForCommandPalette">
    <img src="https://img.shields.io/badge/Install%20with-WinGet-blue.svg" alt="Installa con WinGet">
  </a>
</div>

<div align="center">
  <a href="README.uk.md">
    <img src="https://img.shields.io/badge/docs-Українська-0057B7" alt="Docs Ucraino">
  </a>
  <a href="../README.md">
    <img src="https://img.shields.io/badge/docs-English-2EA44F" alt="Docs Inglese">
  </a>
  <a href="README.fr.md">
    <img src="https://img.shields.io/badge/docs-Français-1F6FEB" alt="Docs Francese">
  </a>
  <a href="README.it.md">
    <img src="https://img.shields.io/badge/docs-Italiano-009246" alt="Docs Italiano">
  </a>
  <a href="README.zh.md">
    <img src="https://img.shields.io/badge/docs-中文-E34C26" alt="Docs Cinese">
  </a>
</div>

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-x64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-x64-blue?style=for-the-badge&logo=github" alt="Scarica x64">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-ARM64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-ARM64-blue?style=for-the-badge&logo=github" alt="Scarica ARM64">
  </a>
    <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/downloads/ruslanlap/PowerToysRun-Definition/total?style=for-the-badge&logo=github" alt="GitHub tutte le release">
  </a>
</div>

## 📋 Indice

- [📋 Panoramica](#-panoramica)
- [✨ Funzionalità](#-funzionalità)
- [🎬 Demo](#-demo)
- [🚀 Installazione](#-installazione)
- [🔧 Utilizzo](#-utilizzo)
- [⚙️ Configurazione](#️-configurazione)
- [📁 Archiviazione Dati](#-archiviazione-dati)
- [🛠️ Compilazione dai Sorgenti](#️-compilazione-dai-sorgenti)
- [📊 Struttura del Progetto](#-struttura-del-progetto)
- [🤝 Contribuire](#-contribuire)
- [❓ FAQ](#-faq)
- [🧑‍💻 Stack Tecnologico](#-stack-tecnologico)
- [🌐 Localizzazione](#-localizzazione)
- [📸 Screenshot](#-screenshot)
- [📄 Licenza](#-licenza)
- [🙏 Ringraziamenti](#-ringraziamenti)
- [☕ Supporto](#-supporto)
- [🆕 Novità (v1.5.1)](#-novità-v151)
- [🆕 Novità (v1.4.0)](#-novità-v140)
- [🆕 Novità (v1.3.3)](#-novità-v133)
- [🆕 Novità (v1.3.2)](#-novità-v132)
- [🆕 Novità (v1.3.1)](#-novità-v131)

## 🆕 Novità (v1.5.1)

- ⌨️ **Supporto ai Sottocomandi** — Accesso rapido a dati specifici della parola senza la ricerca completa della definizione:
  - `def pronunciation <word>` / `def pron <word>` — Mostra solo pronuncia e audio
  - `def synonyms <word>` / `def syn <word>` — Mostra solo sinonimi
  - `def antonyms <word>` / `def ant <word>` — Mostra solo contrari
  - `def examples <word>` / `def ex <word>` — Mostra solo esempi d'uso
  - Predefinito: `def <word>` mostra tutto (definizioni + fonetica + sinonimi + contrari + esempi)
- Funziona in tutte le lingue supportate (Inglese, Francese, Italiano, Ucraino, Cinese)
- Configurabile tramite le impostazioni esistenti `ShowSynonymsInResults`, `ShowAntonymsInResults`, `ShowExamplesInResults`

## 🆕 Novità (v1.5.1)

- 🇮🇹 **Supporto al Dizionario Italiano** — Aggiunte le ricerche italiane tramite Wikizionario (`it.wiktionary.org`)
- 🌐 **Ricerche Latine Ampliate** — Il `LatinLanguages` predefinito ora include Inglese, Francese e Italiano (`"en,fr,it"`)
- ⚙️ **Registrazione Provider** — L'italiano è ora disponibile come provider di dizionario di prima classe

## 🆕 Novità (v1.4.0)

- 🇫🇷 **Supporto al Dizionario Francese** — Aggiunto il supporto al francese tramite il dizionario Collins Francese-Inglese con fallback su Wiktionnaire
- 🤖 **Rilevamento Automatico della Lingua** — Usa query naturali come `def world`, `def Enchanté`, `def слово`
- 🌐 **Ricerche Latine Multilingua** — Configura l'impostazione `LatinLanguages` (ad es. `"en,fr"`) per interrogare contemporaneamente più dizionari in alfabeto latino
- ⚙️ **Configurazione Avanzata** — Aggiunta l'impostazione `LatinLanguages` per una selezione flessibile della lingua
- 🔄 **Instradamento dei Provider Migliorato** — Migore corrispondenza automatica per input multilingua

## 🆕 Novità (v1.3.3)

- 🇺🇦 **Dizionario Ucraino** — Passato a Wiktionary https://uk.wiktionary.org come fonte principale


## 📋 Panoramica

Definition è un plugin per [Microsoft PowerToys Run](https://github.com/microsoft/PowerToys) che permette di cercare rapidamente definizioni di parole, fonetica e sinonimi senza lasciare la tastiera. Basta digitare `def <word>` per ottenere le definizioni. Il plugin supporta **Inglese**, **Francese (Français)**, **Italiano (Italiano)**, **Ucraino (Українська)** e **Cinese (中文)** con rilevamento automatico dell'alfabeto — digita una parola in una qualsiasi delle lingue supportate e il plugin darà priorità ai risultati di conseguenza.

<div align="center">
  <img src="../data/demo-definition-2.gif" alt="Cerca definizioni di parole" width="650">
</div>

## ✨ Funzionalità

- 🔍 **Definizioni Istantanee**: Ottieni definizioni in tempo reale tramite `dictionaryapi.dev`.
- 🇫🇷 **Dizionario Francese (Français)**: Cerca parole francesi tramite Collins con fallback su Wiktionnaire.
- 🇮🇹 **Dizionario Italiano (Italiano)**: Cerca parole italiane tramite Wikizionario.
- 🇺🇦 **Dizionario Ucraino (Українська)**: Cerca parole ucraine usando Wiktionary https://uk.wiktionary.org come fonte principale.
- 🇨🇳 **Dizionario Cinese (中文)**: Ricerche offline Cinese-Inglese basate sul database CC-CEDICT integrato (~124.000 voci) — non serve connessione internet.
- 🔄 **Ricerca Parallela Multilingua**: Tutti i provider configurati vengono interrogati contemporaneamente; i risultati sono classificati in base all'alfabeto della tua query (Latino, Cirillico o caratteri Cinesi).
- 🤖 **Rilevamento Automatico della Lingua**: Usa un input naturale come `def world`, `def Enchanté` o `def слово`.
- 🔊 **Audio della Pronuncia**: Riproduci l'audio fonetico direttamente dai tuoi risultati.
- 📚 **Fonetica e Sinonimi**: Visualizza la trascrizione fonetica, i sinonimi e i contrari.
- 📝 **Esempi d'Uso**: Vedi esempi reali di come vengono usate le parole.
- ⚙️ **Completamente Configurabile**: Configurazione basata su JSON con oltre 15 impostazioni personalizzabili.
- ⏱️ **Esecuzione Ritardata**: Mostra un indicatore di caricamento prima di recuperare i risultati.
- 💾 **Cache Intelligente**: Cache in memoria per ricerche ripetute con dimensione e scadenza configurabili.
- 🔄 **Gestione Robusta della Rete**: Logica di retry con backoff esponenziale per chiamate API affidabili.
- 🌓 **Sensibilità al Tema**: Cambia automaticamente le icone per la modalità chiara/scura.
- 📋 **Menu di Contesto Ricco**: Copia definizioni, riproduci la pronuncia, apri l'URL di origine o cerca parole correlate.
- 🔄 **Richieste Annullabili**: Annulla automaticamente le richieste precedenti quando digiti nuove query.
- 🌐 **Integrazione con Wiktionary**: Apri qualsiasi parola in Wiktionary per informazioni aggiuntive e traduzioni.

## 🎬 Demo

<div align="center">
  <img src="../data/demo-definition.gif" alt="Demo del Plugin Definition" width="650">
</div>

## 🚀 Installazione

### Prerequisiti

- [PowerToys Run](https://github.com/microsoft/PowerToys/releases) installato (v0.70.0 o successiva)
- Windows 10 (build 22621) o successivo
- .NET 9.0 Runtime (incluso in Windows 11 22H2 o successivo)
- Connessione internet (per l'accesso alle API)

### Installazione Rapida (Manuale)

1. Scarica lo ZIP appropriato per l'architettura del tuo sistema:
   - [Versione x64](https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-x64.zip)
   - [Versione ARM64](https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-ARM64.zip)

2. Estrai lo ZIP in:
   ```
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```
   
   Percorso tipico: `C:\Users\TuoNomeUtente\AppData\Local\Microsoft\PowerToys\PowerToys Run\Plugins\`

3. Riavvia PowerToys (fai clic destro sull'icona di PowerToys nella barra delle applicazioni e seleziona "Riavvia").

4. Apri PowerToys Run (`Alt + Space`) e digita `def <word>`.

### Verifica Manuale

Per verificare che il plugin sia installato correttamente:

1. Apri le Impostazioni di PowerToys
2. Vai su PowerToys Run > Plugin
3. Cerca "Definition" nell'elenco dei plugin
4. Assicurati che sia attivato (l'interruttore deve essere su ON)

## 🔧 Utilizzo

1. Attiva PowerToys Run (`Alt + Space`).
2. Digita:
   - `def` per vedere le istruzioni.
   - `def <word>` per cercare le definizioni automaticamente in base alla lingua/alfabeto.
   - **Sottocomandi** (v1.5.1+):
     - `def pronunciation <word>` / `def pron <word>` — mostra solo pronuncia + audio
     - `def synonyms <word>` / `def syn <word>` — mostra solo sinonimi
     - `def antonyms <word>` / `def ant <word>` — mostra solo contrari
     - `def examples <word>` / `def ex <word>` — mostra solo esempi d'uso
3. Premi <kbd>Invio</kbd> per recuperare i risultati.
4. Usa <kbd>Ctrl + C</kbd> per copiare una definizione.
5. Fai clic destro su un risultato per:
   - Copiare la definizione con <kbd>Ctrl + C</kbd>
   - Riprodurre l'audio della pronuncia
   - Aprire la parola in Wiktionary
   - Cercare parole correlate

<div align="center">
  <img src="../data/demo-subcommands.gif" alt="Demo dei Sottocomandi" width="650">
</div>

## ⚙️ Configurazione

Il plugin supporta un'ampia personalizzazione tramite un file `config.json` che viene creato automaticamente nella directory del plugin. Le modifiche hanno effetto immediato senza richiedere un riavvio.

### Impostazioni Disponibili

| Impostazione | Predefinito | Descrizione |
|---------|---------|-------------|
| `Language` | `"en"` | Lingua predefinita (`"en"`, `"fr"`, `"it"`, `"uk"` o `"zh"`) |
| `ApiEndpoint` | `https://api.dictionaryapi.dev/api/v2/entries/en/` | Endpoint API del dizionario inglese |
| `LatinLanguages` | `"en,fr,it"` | Lingue in alfabeto latino da interrogare, separate da virgole (ad es. `"en,fr,it"` per Inglese, Francese e Italiano) |
| `UkrainianApiEndpoint` | `https://sum.in.ua/s/` | Endpoint di fallback del dizionario ucraino (sum.in.ua) |
| `ChineseApiEndpoint` | `https://www.mdbg.net/chinese/dictionary?...` | URL di riferimento del dizionario cinese |
| `CacheMaxSize` | 100 | Numero massimo di ricerche di parole memorizzate nella cache |
| `HttpTimeoutSeconds` | 10 | Timeout per le richieste API in secondi |
| `CacheExpirationMinutes` | 30 | Per quanto tempo mantenere le voci nella cache |
| `EnableAudioPlayback` | true | Abilita/disabilita l'audio della pronuncia |
| `EnableClipboardOperations` | true | Abilita/disabilita la copia negli appunti |
| `TextTruncateLength` | 30 | Lunghezza massima del testo nel menu di contesto |
| `EnableVerboseLogging` | false | Abilita il logging di debug dettagliato |
| `MaxResultsPerMeaning` | 3 | Definizioni massime per significato della parola |
| `ShowExamplesInResults` | true | Mostra esempi d'uso |
| `ShowSynonymsInResults` | true | Mostra sinonimi |
| `ShowAntonymsInResults` | true | Mostra contrari |

### Esempio di Configurazione

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

> **Nota:** Non è necessario modificare `Language` per usare l'Ucraino o il Cinese. Il plugin rileva automaticamente l'alfabeto della tua query. Input cirillico (es. `def слово`) darà priorità ai risultati ucraini, i caratteri cinesi daranno priorità ai risultati cinesi e l'input in alfabeto latino interrogherà le lingue elencate in `LatinLanguages`.
>
> **Ricerche latine multilingua:** Imposta `"LatinLanguages": "en,fr,it"` per interrogare contemporaneamente i dizionari inglese, francese e italiano per parole in alfabeto latino.

## 📁 Archiviazione Dati

Tutte le impostazioni sono memorizzate nel file di impostazioni standard di PowerToys (nessun file di dati aggiuntivo viene creato).

## 🛠️ Compilazione dai Sorgenti

```bash
git clone https://github.com/ruslanlap/PowerToysRun-Definition.git
cd PowerToysRun-Definition/Definition
dotnet build
# Per creare il pacchetto:
dotnet publish -c Release -r win-x64 --output ./publish
zip -r Definition-v1.5.1-x64.zip ./publish
```

## 📊 Struttura del Progetto

```
PowerToysRun-Definition/
├── data/                            # Risorse del plugin (icone, demo)
│   ├── definition.dark.png
│   ├── definition.logo.png
│   ├── demo-definition.gif
│   └── demo-definition-2.gif
├── Definition/                      # Sorgenti del plugin
│   ├── Community.PowerToys.Run.Plugin.Definition/
│   │   ├── Images/
│   │   │   ├── definition.dark.png
│   │   │   └── definition.light.png
│   │   ├── Main.cs
│   │   └── plugin.json
│   └── Community.PowerToys.Run.Plugin.Definition.csproj
└── README.md
```

## 🤝 Contribuire

I contributi sono benvenuti! Ecco come puoi aiutare:

1. Fai un fork del repository
2. Crea un branch per la funzionalità: `git checkout -b feature/funzione-fantastica`
3. Conferma le tue modifiche: `git commit -m 'Aggiungi funzione fantastica'`
4. Invia il branch: `git push origin feature/funzione-fantastica`
5. Apri una Pull Request

Assicurati di aggiornare i test in modo appropriato.

### Contributor

- [ruslanlap](https://github.com/ruslanlap) - Creatore e manutentore del progetto

## ❓ FAQ

<details>
<summary><b>Il plugin richiede l'accesso a internet?</b></summary>
<p>Le ricerche in Inglese, Francese, Italiano e Ucraino richiedono l'accesso a internet (rispettivamente dictionaryapi.dev, collinsdictionary.com/wiktionary, it.wiktionary.org e uk.wiktionary.org). Le ricerche in Cinese utilizzano un dizionario offline integrato e funzionano senza internet. Tutti i risultati vengono memorizzati nella cache in memoria per le ricerche successive.</p>
</details>

<details>
<summary><b>Come cambio il tema del plugin?</b></summary>
<p>Il plugin si adatta automaticamente al tuo tema di PowerToys (chiaro/scuro). Le icone vengono caricate dinamicamente in base al tema attuale del sistema.</p>
</details>

<details>
<summary><b>Le definizioni vengono memorizzate nella cache?</b></summary>
<p>Sì, le definizioni vengono memorizzate nella cache in memoria durante la sessione corrente (fino a 100 voci) per migliorare le prestazioni e ridurre le chiamate API.</p>
</details>

<details>
<summary><b>Posso personalizzare la fonte del dizionario?</b></summary>
<p>Sì. Puoi modificare <code>ApiEndpoint</code> (Inglese) e <code>UkrainianApiEndpoint</code> (Ucraino) nel file <code>config.json</code>. Le ricerche in Cinese utilizzano il database CC-CEDICT integrato.</p>
</details>

<details>
<summary><b>Come cerco le parole ucraine?</b></summary>
<p>Basta digitare <code>def слово</code> (qualsiasi parola ucraina in cirillico). Il plugin rileva automaticamente l'alfabeto cirillico e dà priorità ai risultati ucraini. La fonte principale è <a href="https://goroh.pp.ua/">goroh.pp.ua</a> (Горох — словники української мови, oltre 500.000 parole) con <a href="https://sum.in.ua/">sum.in.ua</a> come fallback. Non è necessaria alcuna chiave API speciale.</p>
</details>

<details>
<summary><b>Quali lingue sono supportate?</b></summary>
<p>Cinque lingue sono supportate già pronte all'uso:</p>
<ul>
<li><strong>Inglese</strong> — tramite <a href="https://dictionaryapi.dev/">dictionaryapi.dev</a> (API REST gratuita)</li>
<li><strong>Francese (Français)</strong> — tramite <a href="https://www.collinsdictionary.com/dictionary/french-english/">Dizionario Collins Francese-Inglese</a> (principale) + <a href="https://fr.wiktionary.org/">Wiktionnaire</a> (fallback)</li>
<li><strong>Italiano (Italiano)</strong> — tramite <a href="https://it.wiktionary.org/">Wikizionario</a></li>
<li><strong>Ucraino (Українська)</strong> — tramite <a href="https://uk.wiktionary.org/">Wiktionary</a> (principale) + <a href="https://goroh.pp.ua/">goroh.pp.ua</a> (fallback)</li>
<li><strong>Cinese (中文)</strong> — tramite database CC-CEDICT integrato (~124.000 voci, completamente offline)</li>
</ul>
</details>

<details>
<summary><b>Perché il plugin mostra "Ricerca in corso..." prima di mostrare i risultati?</b></summary>
<p>Il plugin implementa IDelayedExecutionPlugin che mostra un indicatore di caricamento durante il recupero dei risultati dall'API. Questo fornisce un feedback immediato durante l'elaborazione della richiesta.</p>
</details>

<details>
<summary><b>Come riproduco l'audio della pronuncia?</b></summary>
<p>Fai clic destro su qualsiasi risultato di definizione e seleziona "Riproduci Pronuncia" dal menu di contesto (disponibile solo se l'API fornisce l'audio per quella parola).</p>
</details>

<details>
<summary><b>Come posso vedere maggiori informazioni su una parola?</b></summary>
<p>Fai clic destro su qualsiasi risultato e seleziona "Apri URL di Origine nel Browser" per visualizzare la parola in Wiktionary, che fornisce informazioni aggiuntive, traduzioni ed etimologia.</p>
</details>

<details>
<summary><b>Qual è la differenza tra l'installazione con WinGet e quella manuale?</b></summary>
<p><strong>Installazione con WinGet:</strong> Esegui un comando (<code>winget install ruslanlap.DefinitionForCommandPalette</code>) e WinGet gestisce tutto: scarica, verifica, installa e registra l'estensione automaticamente. Riceverai anche notifiche di aggiornamento automatico quando vengono rilasciate nuove versioni.</p>
<p><strong>Installazione manuale:</strong> Scarica il file ZIP, estrailo in una cartella specifica, riavvia PowerToys. Devi cercare gli aggiornamenti manualmente su GitHub.</p>
<p>WinGet è consigliato alla maggior parte degli utenti poiché è più comodo e garantisce di avere sempre l'ultima versione.</p>
</details>

## 🔆 Funzionalità in Evidenza

Questa sezione mette in evidenza alcune delle funzionalità più potenti del plugin Definition:

<div align="center">
  <figure>
    <img src="../data/demo8.png" width="800" alt="Integrazione con Wiktionary">
    <figcaption>
      <strong>Integrazione con Wiktionary</strong> - Accedi a informazioni complete sulle parole aprendo qualsiasi parola in Wiktionary direttamente dal menu di contesto. Ottieni l'accesso a significati aggiuntivi, traduzioni, etimologie e termini correlati.
    </figcaption>
  </figure>
  
  <figure>
    <img src="../data/demo9.png" width="800" alt="Menu di Contesto Avanzato">
    <figcaption><strong>Menu di Contesto Ricco</strong> - Il plugin offre un potente menu di contesto con più azioni. 
      Copia definizioni, riproduci l'audio della pronuncia, apri gli URL di origine e cerca parole correlate. 
      Fai clic destro su qualsiasi risultato per accedere a queste funzionalità.
    </figcaption>
  </figure>
</div>

## 🧑‍💻 Stack Tecnologico

| Tecnologia | Descrizione |
|---|---|
| C# / .NET 9.0 | Linguaggio principale e runtime |
| PowerToys Run API | Interfacce IPlugin, IDelayedExecutionPlugin, IContextMenu |
| HttpClient | Richieste API con gestione dei timeout |
| System.Text.Json | Analisi JSON |
| WPF MediaPlayer | Riproduzione audio |
| System.Threading | Operazioni asincrone |
| GitHub Actions | CI/CD con build multi-architettura |

## 🌐 Lingue Supportate

Il plugin supporta quattro fonti di dizionari con rilevamento automatico dell'alfabeto:

| Lingua | Fonte | Metodo | Internet Richiesto |
|----------|--------|--------|:-----------------:|
| **Inglese** | [dictionaryapi.dev](https://dictionaryapi.dev/) | API REST (JSON) | Sì |
| **Français** | [Collins](https://www.collinsdictionary.com/dictionary/french-english/) (principale) + [Wiktionnaire](https://fr.wiktionary.org/) (fallback) | Analisi HTML + API MediaWiki | Sì |
| **Українська** | [Wiktionary](https://uk.wiktionary.org/) (principale) + [goroh.pp.ua](https://goroh.pp.ua/) (fallback) | API + scraping HTML | Sì |
| **中文** | CC-CEDICT (integrato, ~124.000 voci) | Database offline | No |

**Come funziona:** Quando digiti `def <word>`, il plugin rileva l'alfabeto del tuo input e interroga i provider appropriati:
- Input cirillico (`def слово`) → Risultati ucraini prioritizzati
- Caratteri cinesi (`def 你好`) → Risultati cinesi prioritizzati
- Input in alfabeto latino (`def hello` / `def enchanté`) → Interroga le lingue dalla configurazione `LatinLanguages` (predefinito: Inglese + Francese)

> **Nota sull'Ucraino:** Non esiste un'API REST pubblica per i dizionari ucraini. Il plugin utilizza [goroh.pp.ua](https://goroh.pp.ua/) (Горох — українські словники) come fonte principale — un dizionario ucraino completo con oltre 500.000 parole, definizioni, esempi, sinonimi e altro. Le parole cirilliche vengono usate direttamente nell'URL (ad es. `def слово` → `https://goroh.pp.ua/Тлумачення/слово`). Se goroh.pp.ua non è disponibile, viene utilizzato [sum.in.ua](https://sum.in.ua/) come fallback.

## 📸 Screenshot

<div style="display:flex;flex-wrap:wrap;justify-content:center;gap:20px;">
  <figure style="margin:0;">
    <img src="../data/demo1.png" width="300" alt="Definizione di Parola">
    <figcaption style="text-align:center;">Definizione di Parola</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo2.png" width="300" alt="Visualizzazione Fonetica">
    <figcaption style="text-align:center;">Visualizzazione Fonetica</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo3.png" width="300" alt="Menu di Contesto">
    <figcaption style="text-align:center;">Menu di Contesto</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo4.png" width="300" alt="Funzione Contrari">
    <figcaption style="text-align:center;">Funzione Contrari</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo5.png" width="300" alt="Pronuncia Audio">
    <figcaption style="text-align:center;">Pronuncia Audio</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo6.png" width="300" alt="Esecuzione Ritardata">
    <figcaption style="text-align:center;">Esecuzione Ritardata</figcaption>
  </figure>
</div>

## 📄 Licenza

Questo progetto è distribuito con licenza MIT - vedi il file [LICENSE](LICENSE) per i dettagli.

## 🙏 Ringraziamenti

- Il team di [Microsoft PowerToys](https://github.com/microsoft/PowerToys) per l'incredibile launcher
- [dictionaryapi.dev](https://dictionaryapi.dev/) per aver fornito l'API gratuita del dizionario inglese
- [Dizionario Collins](https://www.collinsdictionary.com/dictionary/french-english/) per i contenuti del dizionario Francese-Inglese
- [Wiktionnaire](https://fr.wiktionary.org/) per le definizioni francesi di fallback
- [goroh.pp.ua](https://goroh.pp.ua/) per Горох — українські словники (fonte principale del dizionario ucraino) NECESSARIA API: scrivere agli sviluppatori di goroh.pp.ua per aggiungere l'API al plugin. 
- [sum.in.ua](https://sum.in.ua/) per il Словник української мови (dizionario ucraino di fallback) NON FUNZIONANTE.
- [MDBG.net](https://www.mdbg.net) per l'accesso al dizionario CC-CEDICT Cinese-Inglese
- [Wiktionary](https://en.wiktionary.org/) per informazioni complete sulle parole e traduzioni
- Tutti i contributori che hanno contribuito a migliorare questo plugin

## ☕ Supporto

Se trovi utile questo plugin e vuoi supportarne lo sviluppo, puoi offrirmi un caffè:

[![Offrimi un caffè](https://img.shields.io/badge/Buy%20me%20a%20coffee-☕️-FFDD00?style=for-the-badge&logo=buy-me-a-coffee)](https://ruslanlap.github.io/ruslanlap_buymeacoffe/)

## 🆕 Novità (v1.2.2)

- 🇺🇦 **Supporto al Dizionario Ucraino** — Integrato con il dizionario esplicativo `sum.in.ua`. NON FUNZIONANTE.
- 🇨🇳 **Supporto al Dizionario Cinese** — Integrato con `MDBG.net` (dati CC-CEDICT) per le ricerche Cinese-Inglese.
- 🔄 **Ricerca Parallela** — Recupera simultaneamente i risultati da fonti inglesi, ucraine e cinesi.
- 🎯 **Prioritizzazione Intelligente** — I risultati vengono automaticamente classificati in base all'alfabeto della query (cirillico, cinese o latino).
- 🏗️ **Architettura Migliorata** — Rifattorizzato in un sistema basato su provider per una migliore estensibilità.
- 🩹 **Maggiore Affidabilità** — Una migliore gestione degli errori garantisce che il fallimento di un provider non comprometta l'intera ricerca.
- 📦 **Dipendenze** — Aggiunto `HtmlAgilityPack` per un'analisi HTML robusta dei risultati ucraini e cinesi.

## 🆕 Novità (v1.2.1)

- ⚙️ **Impostazioni Completamente Configurabili** — Sistema di configurazione basato su JSON con aggiornamenti in tempo reale:
  - `config.json` con 11 impostazioni personalizzabili
  - Attiva/disattiva la visualizzazione di sinonimi, contrari, esempi
  - Configura la dimensione della cache, i timeout e i limiti dei risultati
  - Abilita/disabilita la riproduzione audio e le operazioni negli appunti
  - Le impostazioni si ricaricano automaticamente senza riavvio
- 🔄 **Logica di Retry di Rete Robusta** — Affidabilità migliorata per le chiamate API:
  - Backoff esponenziale con condizioni intelligenti di retry
  - Gestisce con eleganza gli errori di rete transitori
  - Tentativi e ritardi di retry configurabili
- 🛠️ **Operazioni negli Appunti Migliorate** — Migliore threading e affidabilità:
  - Task scheduler STA personalizzato per la thread safety
  - Gestione degli errori migliorata e protezione dai timeout
  - Operazioni negli appunti configurabili (abilita/disabilita)
- 🔧 **Correzione Bug di Configurazione** — Ora le impostazioni funzionano davvero:
  - Risolto il problema per cui le modifiche a config.json venivano ignorate
  - Tutte le opzioni di configurazione ora vengono rispettate correttamente
  - Il ricaricamento dinamico garantisce un effetto immediato
- 📊 **Debug Migliorato** — Migliori capacità di risoluzione dei problemi:
  - Opzione di logging dettagliato per diagnostiche approfondite
  - Segnalazione degli errori migliorata in tutto il plugin
  - Migliore categorizzazione degli errori di rete

---

<div align="center">
  <sub>Fatto con ❤️ da <a href="https://github.com/ruslanlap">ruslanlap</a></sub>
</div>
