# 🔍 PowerToys Run : Plugin Definition

<div align="center">
  <img src="../data/definition.logo.png" alt="Logo du plugin Definition" width="128" height="128">
</div>

<div align="center">
  <h1>Definition</h1>
  <p>Recherchez des définitions de mots, la phonétique et les synonymes directement dans PowerToys Run.</p>
  <img src="../data/demo-definition.gif" alt="Démonstration du plugin Definition" width="650">
</div>



<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml">
    <img src="https://github.com/ruslanlap/PowerToysRun-Definition/actions/workflows/build-and-release.yml/badge.svg" alt="Statut du build">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/v/release/ruslanlap/PowerToysRun-Definition?label=latest" alt="Dernière version">
  </a>
  <img src="https://img.shields.io/badge/version-v1.5.1-brightgreen" alt="Version">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/stargazers">
    <img src="https://img.shields.io/github/stars/ruslanlap/PowerToysRun-Definition" alt="Étoiles GitHub">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/issues">
    <img src="https://img.shields.io/github/issues/ruslanlap/PowerToysRun-Definition" alt="Issues GitHub">
  </a>
  <a href="https://opensource.org/licenses/MIT">
    <img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="Licence">
      <img src="https://img.shields.io/badge/Made%20with-❤️-red" alt="Fait avec amour">
  <img src="https://img.shields.io/badge/Awesome-Yes-orange" alt="Awesome">
          <a href="https://github.com/hlaueriksson/awesome-powertoys-run-plugins">
    <img src="https://awesome.re/mentioned-badge.svg" alt="Mentionné dans Awesome PowerToys Run Plugins">
  </a>
  <a href="https://winstall.app/apps/ruslanlap.DefinitionForCommandPalette">
    <img src="https://img.shields.io/badge/Install%20with-WinGet-blue.svg" alt="Installer avec WinGet">
  </a>
</div>

<div align="center">
  <a href="README.uk.md">
    <img src="https://img.shields.io/badge/docs-Українська-0057B7" alt="Documentation en ukrainien">
  </a>
  <a href="../README.md">
    <img src="https://img.shields.io/badge/docs-English-2EA44F" alt="Documentation en anglais">
  </a>
  <a href="README.fr.md">
    <img src="https://img.shields.io/badge/docs-Français-1F6FEB" alt="Documentation en français">
  </a>
  <a href="README.it.md">
    <img src="https://img.shields.io/badge/docs-Italiano-009246" alt="Documentation en italien">
  </a>
  <a href="README.zh.md">
    <img src="https://img.shields.io/badge/docs-中文-E34C26" alt="Documentation en chinois">
  </a>
</div>

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-x64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-x64-blue?style=for-the-badge&logo=github" alt="Télécharger x64">
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-ARM64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-ARM64-blue?style=for-the-badge&logo=github" alt="Télécharger ARM64">
  </a>
    <a href="https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest">
    <img src="https://img.shields.io/github/downloads/ruslanlap/PowerToysRun-Definition/total?style=for-the-badge&logo=github" alt="Toutes les versions GitHub">
  </a>
</div>

## 📋 Table des matières

- [📋 Présentation](#-présentation)
- [✨ Fonctionnalités](#-fonctionnalités)
- [🎬 Démonstration](#-démonstration)
- [🚀 Installation](#-installation)
- [🔧 Utilisation](#-utilisation)
- [⚙️ Configuration](#️-configuration)
- [📁 Stockage des données](#-stockage-des-données)
- [🛠️ Compilation depuis les sources](#️-compilation-depuis-les-sources)
- [📊 Structure du projet](#-structure-du-projet)
- [🤝 Contribution](#-contribution)
- [❓ FAQ](#-faq)
- [🧑‍💻 Pile technique](#-pile-technique)
- [🌐 Langues prises en charge](#-langues-prises-en-charge)
- [📸 Captures d'écran](#-captures-décran)
- [📄 Licence](#-licence)
- [🙏 Remerciements](#-remerciements)
- [☕ Soutien](#-soutien)
- [🆕 Nouveautés (v1.5.1)](#-nouveautés-v151)
- [🆕 Nouveautés (v1.4.0)](#-nouveautés-v140)
- [🆕 Nouveautés (v1.3.3)](#-nouveautés-v133)
- [🆕 Nouveautés (v1.3.2)](#-nouveautés-v132)
- [🆕 Nouveautés (v1.3.1)](#-nouveautés-v131)

## 🆕 Nouveautés (v1.5.1)

- ⌨️ **Prise en charge des sous-commandes** — Accès rapide à des données spécifiques d'un mot sans afficher la définition complète :
  - `def pronunciation <mot>` / `def pron <mot>` — Afficher uniquement la prononciation et l'audio
  - `def synonyms <mot>` / `def syn <mot>` — Afficher uniquement les synonymes
  - `def antonyms <mot>` / `def ant <mot>` — Afficher uniquement les antonymes
  - `def examples <mot>` / `def ex <mot>` — Afficher uniquement les exemples d'utilisation
  - Par défaut : `def <mot>` affiche tout (définitions + phonétique + synonymes + antonymes + exemples)
- Fonctionne avec toutes les langues prises en charge (anglais, français, italien, ukrainien, chinois)
- Configurable via les réglages existants `ShowSynonymsInResults`, `ShowAntonymsInResults`, `ShowExamplesInResults`

## 🆕 Nouveautés (v1.5.1)

- 🇮🇹 **Prise en charge du dictionnaire italien** — Ajout des recherches en italien via Wikizionario (`it.wiktionary.org`)
- 🌐 **Recherches latines étendues** — Le paramètre `LatinLanguages` inclut désormais par défaut l'anglais, le français et l'italien (`"en,fr,it"`)
- ⚙️ **Enregistrement du fournisseur** — L'italien est désormais disponible comme fournisseur de dictionnaire de premier niveau

## 🆕 Nouveautés (v1.4.0)

- 🇫🇷 **Prise en charge du dictionnaire français** — Ajout du français via le dictionnaire Collins français-anglais avec repli sur Wiktionnaire
- 🤖 **Détection automatique de la langue** — Utilisez des requêtes naturelles comme `def world`, `def Enchanté`, `def слово`
- 🌐 **Recherches latines multilingues** — Configurez le paramètre `LatinLanguages` (par ex. `"en,fr"`) pour interroger simultanément plusieurs dictionnaires en écriture latine
- ⚙️ **Configuration améliorée** — Ajout du paramètre `LatinLanguages` pour une sélection de langue flexible
- 🔄 **Routage des fournisseurs amélioré** — Meilleure correspondance automatique pour les entrées multilingues

## 🆕 Nouveautés (v1.3.3)

- 🇺🇦 **Dictionnaire ukrainien** — Passage à Wiktionary https://uk.wiktionary.org comme source principale


## 📋 Présentation

Definition est un plugin pour [Microsoft PowerToys Run](https://github.com/microsoft/PowerToys) qui vous permet de rechercher rapidement des définitions de mots, la phonétique et les synonymes sans quitter votre clavier. Tapez simplement `def <mot>` pour récupérer les définitions. Le plugin prend en charge l'**anglais**, le **français (Français)**, l'**italien (Italiano)**, l'**ukrainien (Українська)** et le **chinois (中文)** avec détection automatique du système d'écriture — tapez un mot dans n'importe quelle langue prise en charge et le plugin priorisera les résultats en conséquence.

<div align="center">
  <img src="../data/demo-definition-2.gif" alt="Recherche de définitions de mots" width="650">
</div>

## ✨ Fonctionnalités

- 🔍 **Définitions instantanées** : Obtenez des définitions en temps réel via `dictionaryapi.dev`.
- 🇫🇷 **Dictionnaire français (Français)** : Recherchez des mots français via Collins avec repli sur Wiktionnaire.
- 🇮🇹 **Dictionnaire italien (Italiano)** : Recherchez des mots italiens via Wikizionario.
- 🇺🇦 **Dictionnaire ukrainien (Українська)** : Recherchez des mots ukrainiens via Wiktionary https://uk.wiktionary.org comme source principale.
- 🇨🇳 **Dictionnaire chinois (中文)** : Recherches chinois-anglais hors ligne alimentées par la base de données CC-CEDICT intégrée (~124 000 entrées) — aucune connexion réseau requise.
- 🔄 **Recherche multilingue parallèle** : Tous les fournisseurs configurés sont interrogés simultanément ; les résultats sont priorisés selon le système d'écriture de votre requête (latin, cyrillique ou caractères chinois).
- 🤖 **Détection automatique de la langue** : Utilisez une saisie naturelle comme `def world`, `def Enchanté` ou `def слово`.
- 🔊 **Audio de prononciation** : Lisez l'audio phonétique directement depuis vos résultats.
- 📚 **Phonétique et synonymes** : Affichez la transcription phonétique, les synonymes et les antonymes.
- 📝 **Exemples d'utilisation** : Consultez des exemples concrets de l'utilisation des mots.
- ⚙️ **Entièrement configurable** : Configuration basée sur JSON avec plus de 15 paramètres personnalisables.
- ⏱️ **Exécution différée** : Affiche un indicateur de chargement avant de récupérer les résultats.
- 💾 **Cache intelligent** : Cache en mémoire pour les recherches répétées avec taille et expiration configurables.
- 🔄 **Gestion réseau robuste** : Logique de nouvelle tentative avec temporisation exponentielle pour des appels API fiables.
- 🌓 **Sensibilité au thème** : Bascule automatiquement les icônes selon le mode clair/sombre.
- 📋 **Menu contextuel riche** : Copier les définitions, lire la prononciation, ouvrir l'URL source ou rechercher des mots apparentés.
- 🔄 **Requêtes annulables** : Annule automatiquement les requêtes précédentes lors de la saisie de nouvelles recherches.
- 🌐 **Intégration Wiktionary** : Ouvrez n'importe quel mot dans Wiktionary pour des informations et traductions supplémentaires.

## 🎬 Démonstration

<div align="center">
  <img src="../data/demo-definition.gif" alt="Démonstration du plugin Definition" width="650">
</div>

## 🚀 Installation

### Prérequis

- [PowerToys Run](https://github.com/microsoft/PowerToys/releases) installé (v0.70.0 ou ultérieure)
- Windows 10 (build 22621) ou ultérieur
- .NET 9.0 Runtime (inclus avec Windows 11 22H2 ou ultérieur)
- Connexion Internet (pour l'accès à l'API)

### Installation rapide (manuelle)

1. Téléchargez le ZIP approprié à l'architecture de votre système :
   - [Version x64](https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-x64.zip)
   - [Version ARM64](https://github.com/ruslanlap/PowerToysRun-Definition/releases/download/v1.5.1/Definition-1.5.1-ARM64.zip)

2. Extrayez le ZIP vers :
   ```
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```

   Chemin typique : `C:\Users\VotreNomUtilisateur\AppData\Local\Microsoft\PowerToys\PowerToys Run\Plugins\`

3. Redémarrez PowerToys (clic droit sur l'icône PowerToys dans la zone de notification et sélectionnez « Redémarrer »).

4. Ouvrez PowerToys Run (`Alt + Espace`) et tapez `def <mot>`.

### Vérification manuelle

Pour vérifier que le plugin est correctement installé :

1. Ouvrez les paramètres de PowerToys
2. Allez dans PowerToys Run > Plugins
3. Recherchez « Definition » dans la liste des plugins
4. Assurez-vous qu'il est activé (l'interrupteur doit être sur ON)

## 🔧 Utilisation

1. Activez PowerToys Run (`Alt + Espace`).
2. Tapez :
   - `def` pour voir les instructions.
   - `def <mot>` pour rechercher des définitions automatiquement selon la langue/le système d'écriture.
   - **Sous-commandes** (v1.5.1+) :
     - `def pronunciation <mot>` / `def pron <mot>` — afficher uniquement la prononciation + l'audio
     - `def synonyms <mot>` / `def syn <mot>` — afficher uniquement les synonymes
     - `def antonyms <mot>` / `def ant <mot>` — afficher uniquement les antonymes
     - `def examples <mot>` / `def ex <mot>` — afficher uniquement les exemples d'utilisation
3. Appuyez sur <kbd>Entrée</kbd> pour récupérer les résultats.
4. Utilisez <kbd>Ctrl + C</kbd> pour copier une définition.
5. Faites un clic droit sur un résultat pour :
   - Copier la définition avec <kbd>Ctrl + C</kbd>
   - Lire l'audio de prononciation
   - Ouvrir le mot dans Wiktionary
   - Rechercher des mots apparentés

<div align="center">
  <img src="../data/demo-subcommands.gif" alt="Démonstration des sous-commandes" width="650">
</div>

## ⚙️ Configuration

Le plugin prend en charge une personnalisation étendue via un fichier `config.json` qui est automatiquement créé dans le répertoire du plugin. Les modifications prennent effet immédiatement sans nécessiter de redémarrage.

### Paramètres disponibles

| Paramètre | Défaut | Description |
|---------|---------|-------------|
| `Language` | `"en"` | Langue par défaut (`"en"`, `"fr"`, `"it"`, `"uk"` ou `"zh"`) |
| `ApiEndpoint` | `https://api.dictionaryapi.dev/api/v2/entries/en/` | Endpoint de l'API du dictionnaire anglais |
| `LatinLanguages` | `"en,fr,it"` | Langues en écriture latine à interroger, séparées par des virgules (par ex. `"en,fr,it"` pour l'anglais, le français et l'italien) |
| `UkrainianApiEndpoint` | `https://sum.in.ua/s/` | Endpoint de repli du dictionnaire ukrainien (sum.in.ua) |
| `ChineseApiEndpoint` | `https://www.mdbg.net/chinese/dictionary?...` | URL de référence du dictionnaire chinois |
| `CacheMaxSize` | 100 | Nombre maximum de recherches de mots en cache |
| `HttpTimeoutSeconds` | 10 | Délai d'expiration des requêtes API en secondes |
| `CacheExpirationMinutes` | 30 | Durée de conservation des entrées de cache |
| `EnableAudioPlayback` | true | Activer/désactiver l'audio de prononciation |
| `EnableClipboardOperations` | true | Activer/désactiver la copie vers le presse-papiers |
| `TextTruncateLength` | 30 | Longueur maximale du texte dans le menu contextuel |
| `EnableVerboseLogging` | false | Activer la journalisation de débogage détaillée |
| `MaxResultsPerMeaning` | 3 | Nombre maximum de définitions par sens d'un mot |
| `ShowExamplesInResults` | true | Afficher les exemples d'utilisation |
| `ShowSynonymsInResults` | true | Afficher les synonymes |
| `ShowAntonymsInResults` | true | Afficher les antonymes |

### Exemple de configuration

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

> **Remarque :** Vous n'avez pas besoin de modifier `Language` pour utiliser l'ukrainien ou le chinois. Le plugin détecte automatiquement le système d'écriture de votre requête. Une entrée en cyrillique (par ex. `def слово`) priorisera les résultats ukrainiens, les caractères chinois prioriseront les résultats chinois, et une entrée latine interrogera les langues listées dans `LatinLanguages`.
>
> **Recherches latines multilingues :** Définissez `"LatinLanguages": "en,fr,it"` pour interroger simultanément les dictionnaires anglais, français et italien pour les mots en écriture latine.

## 📁 Stockage des données

Tous les paramètres sont stockés dans le fichier de paramètres standard de PowerToys (aucun fichier de données supplémentaire n'est créé).

## 🛠️ Compilation depuis les sources

```bash
git clone https://github.com/ruslanlap/PowerToysRun-Definition.git
cd PowerToysRun-Definition/Definition
dotnet build
# Pour empaqueter :
dotnet publish -c Release -r win-x64 --output ./publish
zip -r Definition-v1.5.1-x64.zip ./publish
```

## 📊 Structure du projet

```
PowerToysRun-Definition/
├── data/                            # Ressources du plugin (icônes, démos)
│   ├── definition.dark.png
│   ├── definition.logo.png
│   ├── demo-definition.gif
│   └── demo-definition-2.gif
├── Definition/                      # Code source du plugin
│   ├── Community.PowerToys.Run.Plugin.Definition/
│   │   ├── Images/
│   │   │   ├── definition.dark.png
│   │   │   └── definition.light.png
│   │   ├── Main.cs
│   │   └── plugin.json
│   └── Community.PowerToys.Run.Plugin.Definition.csproj
└── README.md
```

## 🤝 Contribution

Les contributions sont les bienvenues ! Voici comment vous pouvez aider :

1. Forkez le dépôt
2. Créez une branche de fonctionnalité : `git checkout -b feature/amazing-feature`
3. Commitez vos modifications : `git commit -m 'Add amazing feature'`
4. Poussez vers la branche : `git push origin feature/amazing-feature`
5. Ouvrez une Pull Request

N'oubliez pas de mettre à jour les tests le cas échéant.

### Contributeurs

- [ruslanlap](https://github.com/ruslanlap) - Créateur et mainteneur du projet

## ❓ FAQ

<details>
<summary><b>Le plugin nécessite-t-il un accès Internet ?</b></summary>
<p>Les recherches en anglais, français, italien et ukrainien nécessitent un accès Internet (respectivement dictionaryapi.dev, collinsdictionary.com/wiktionary, it.wiktionary.org et uk.wiktionary.org). Les recherches en chinois utilisent un dictionnaire hors ligne intégré et fonctionnent sans Internet. Tous les résultats sont mis en cache en mémoire pour les recherches ultérieures.</p>
</details>

<details>
<summary><b>Comment changer le thème du plugin ?</b></summary>
<p>Le plugin s'adapte automatiquement à votre thème PowerToys (clair/sombre). Les icônes sont chargées dynamiquement selon le thème actuel de votre système.</p>
</details>

<details>
<summary><b>Les définitions sont-elles mises en cache ?</b></summary>
<p>Oui, les définitions sont mises en cache en mémoire pendant la session en cours (jusqu'à 100 entrées) afin d'améliorer les performances et de réduire les appels API.</p>
</details>

<details>
<summary><b>Puis-je personnaliser la source du dictionnaire ?</b></summary>
<p>Oui. Vous pouvez modifier <code>ApiEndpoint</code> (anglais) et <code>UkrainianApiEndpoint</code> (ukrainien) dans <code>config.json</code>. Les recherches en chinois utilisent la base de données CC-CEDICT intégrée.</p>
</details>

<details>
<summary><b>Comment rechercher des mots ukrainiens ?</b></summary>
<p>Tapez simplement <code>def слово</code> (n'importe quel mot ukrainien en cyrillique). Le plugin détecte automatiquement l'écriture cyrillique et priorise les résultats ukrainiens. La source principale est <a href="https://goroh.pp.ua/">goroh.pp.ua</a> (Горох — українські словники, plus de 500 000 mots) avec <a href="https://sum.in.ua/">sum.in.ua</a> en repli. Aucune clé API spéciale n'est nécessaire.</p>
</details>

<details>
<summary><b>Quelles langues sont prises en charge ?</b></summary>
<p>Cinq langues sont prises en charge nativement :</p>
<ul>
<li><strong>Anglais</strong> — via <a href="https://dictionaryapi.dev/">dictionaryapi.dev</a> (API REST gratuite)</li>
<li><strong>Français (Français)</strong> — via <a href="https://www.collinsdictionary.com/dictionary/french-english/">Collins French-English Dictionary</a> (principal) + <a href="https://fr.wiktionary.org/">Wiktionnaire</a> (repli)</li>
<li><strong>Italien (Italiano)</strong> — via <a href="https://it.wiktionary.org/">Wikizionario</a></li>
<li><strong>Ukrainien (Українська)</strong> — via <a href="https://uk.wiktionary.org/">Wiktionary</a> (principal) + <a href="https://goroh.pp.ua/">goroh.pp.ua</a> (repli)</li>
<li><strong>Chinois (中文)</strong> — via la base de données CC-CEDICT intégrée (~124 000 entrées, entièrement hors ligne)</li>
</ul>
</details>

<details>
<summary><b>Pourquoi le plugin affiche-t-il « Looking up... » avant d'afficher les résultats ?</b></summary>
<p>Le plugin implémente IDelayedExecutionPlugin qui affiche un indicateur de chargement pendant la récupération des résultats depuis l'API. Cela fournit un retour immédiat pendant le traitement de la requête.</p>
</details>

<details>
<summary><b>Comment lire l'audio de prononciation ?</b></summary>
<p>Faites un clic droit sur n'importe quel résultat de définition et sélectionnez « Lire la prononciation » dans le menu contextuel (disponible uniquement si l'API fournit un audio pour ce mot).</p>
</details>

<details>
<summary><b>Comment puis-je voir plus d'informations sur un mot ?</b></summary>
<p>Faites un clic droit sur n'importe quel résultat et sélectionnez « Ouvrir l'URL source dans le navigateur » pour consulter le mot dans Wiktionary, qui fournit des informations supplémentaires, des traductions et l'étymologie.</p>
</details>

<details>
<summary><b>Quelle est la différence entre WinGet et l'installation manuelle ?</b></summary>
<p><strong>Installation via WinGet :</strong> Exécutez une seule commande (<code>winget install ruslanlap.DefinitionForCommandPalette</code>) et WinGet s'occupe de tout — téléchargement, vérification, installation et enregistrement automatique de l'extension. Vous recevez également des notifications de mise à jour automatique lors de la sortie de nouvelles versions.</p>
<p><strong>Installation manuelle :</strong> Téléchargez le fichier ZIP, extrayez-le dans un dossier spécifique, redémarrez PowerToys. Vous devez vérifier manuellement les mises à jour sur GitHub.</p>
<p>WinGet est recommandé pour la plupart des utilisateurs car il est plus pratique et garantit que vous disposez toujours de la dernière version.</p>
</details>

## 🔆 Zoom sur les fonctionnalités

Cette section met en lumière certaines des fonctionnalités les plus puissantes du plugin Definition :

<div align="center">
  <figure>
    <img src="../data/demo8.png" width="800" alt="Intégration Wiktionary">
    <figcaption>
      <strong>Intégration Wiktionary</strong> - Accédez à des informations complètes sur les mots en ouvrant n'importe quel mot dans Wiktionary directement depuis le menu contextuel. Obtenez accès à des sens supplémentaires, des traductions, des étymologies et des termes apparentés.
    </figcaption>
  </figure>

  <figure>
    <img src="../data/demo9.png" width="800" alt="Menu contextuel avancé">
    <figcaption><strong>Menu contextuel riche</strong> - Le plugin offre un menu contextuel puissant avec plusieurs actions.
      Copiez des définitions, lisez l'audio de prononciation, ouvrez les URL source et recherchez des mots apparentés.
      Faites un clic droit sur n'importe quel résultat pour accéder à ces fonctionnalités.
    </figcaption>
  </figure>
</div>

## 🧑‍💻 Pile technique

| Technologie | Description |
|---|---|
| C# / .NET 9.0 | Langage principal et runtime |
| API PowerToys Run | Interfaces IPlugin, IDelayedExecutionPlugin, IContextMenu |
| HttpClient | Requêtes API avec gestion des délais d'expiration |
| System.Text.Json | Analyse JSON |
| WPF MediaPlayer | Lecture audio |
| System.Threading | Opérations asynchrones |
| GitHub Actions | CI/CD avec builds multi-architecture |

## 🌐 Langues prises en charge

Le plugin prend en charge quatre sources de dictionnaires avec détection automatique du système d'écriture :

| Langue | Source | Méthode | Internet requis |
|----------|--------|--------|:-----------------:|
| **Anglais** | [dictionaryapi.dev](https://dictionaryapi.dev/) | API REST (JSON) | Oui |
| **Français** | [Collins](https://www.collinsdictionary.com/dictionary/french-english/) (principal) + [Wiktionnaire](https://fr.wiktionary.org/) (repli) | Analyse HTML + API MediaWiki | Oui |
| **Українська** | [Wiktionary](https://uk.wiktionary.org/) (principal) + [goroh.pp.ua](https://goroh.pp.ua/) (repli) | API + scraping HTML | Oui |
| **中文** | CC-CEDICT (intégré, ~124 000 entrées) | Base de données hors ligne | Non |

**Comment ça marche :** Quand vous tapez `def <mot>`, le plugin détecte le système d'écriture de votre entrée et interroge les fournisseurs appropriés :
- Entrée cyrillique (`def слово`) → Résultats ukrainiens priorisés
- Caractères chinois (`def 你好`) → Résultats chinois priorisés
- Entrée latine (`def hello` / `def enchanté`) → Interroge les langues de la configuration `LatinLanguages` (par défaut : anglais + français)

> **Remarque sur l'ukrainien :** Il n'existe pas d'API REST publique pour les dictionnaires ukrainiens. Le plugin utilise [goroh.pp.ua](https://goroh.pp.ua/) (Горох — українські словники) comme source principale — un dictionnaire ukrainien complet avec plus de 500 000 mots, définitions, exemples, synonymes et plus encore. Les mots en cyrillique sont utilisés directement dans l'URL (par ex. `def слово` → `https://goroh.pp.ua/Тлумачення/слово`). Si goroh.pp.ua est indisponible, [sum.in.ua](https://sum.in.ua/) est utilisé en repli.

## 📸 Captures d'écran

<div style="display:flex;flex-wrap:wrap;justify-content:center;gap:20px;">
  <figure style="margin:0;">
    <img src="../data/demo1.png" width="300" alt="Définition de mot">
    <figcaption style="text-align:center;">Définition de mot</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo2.png" width="300" alt="Affichage de la phonétique">
    <figcaption style="text-align:center;">Affichage de la phonétique</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo3.png" width="300" alt="Menu contextuel">
    <figcaption style="text-align:center;">Menu contextuel</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo4.png" width="300" alt="Fonctionnalité antonymes">
    <figcaption style="text-align:center;">Fonctionnalité antonymes</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo5.png" width="300" alt="Prononciation audio">
    <figcaption style="text-align:center;">Prononciation audio</figcaption>
  </figure>
  <figure style="margin:0;">
    <img src="../data/demo6.png" width="300" alt="Exécution différée">
    <figcaption style="text-align:center;">Exécution différée</figcaption>
  </figure>
</div>

## 📄 Licence

Ce projet est sous licence MIT — voir le fichier [LICENSE](../LICENSE) pour plus de détails.

## 🙏 Remerciements

- L'équipe [Microsoft PowerToys](https://github.com/microsoft/PowerToys) pour le lanceur incroyable
- [dictionaryapi.dev](https://dictionaryapi.dev/) pour la fourniture de l'API gratuite de dictionnaire anglais
- [Collins Dictionary](https://www.collinsdictionary.com/dictionary/french-english/) pour le contenu du dictionnaire français-anglais
- [Wiktionnaire](https://fr.wiktionary.org/) pour les définitions de repli en français
- [goroh.pp.ua](https://goroh.pp.ua/) pour Горох — українські словники (source principale du dictionnaire ukrainien) BESOIN D'API écrire aux développeurs de goroh.pp.ua pour ajouter l'API au plugin.
- [sum.in.ua](https://sum.in.ua/) pour le Словник української мови (dictionnaire ukrainien de repli) NE FONCTIONNE PAS.
- [MDBG.net](https://www.mdbg.net) pour l'accès au dictionnaire chinois-anglais CC-CEDICT
- [Wiktionary](https://en.wiktionary.org/) pour les informations complètes sur les mots et les traductions
- Tous les contributeurs qui ont aidé à améliorer ce plugin

## ☕ Soutien

Si vous trouvez ce plugin utile et souhaitez soutenir son développement, vous pouvez m'offrir un café :

[![Offrez-moi un café](https://img.shields.io/badge/Buy%20me%20a%20coffee-☕️-FFDD00?style=for-the-badge&logo=buy-me-a-coffee)](https://ruslanlap.github.io/ruslanlap_buymeacoffe/)

## 🆕 Nouveautés (v1.2.2)

- 🇺🇦 **Prise en charge du dictionnaire ukrainien** — Intégration avec le dictionnaire explicatif `sum.in.ua`. NE FONCTIONNE PAS.
- 🇨🇳 **Prise en charge du dictionnaire chinois** — Intégration avec `MDBG.net` (données CC-CEDICT) pour les recherches chinois-anglais.
- 🔄 **Recherche parallèle** — Récupère simultanément les résultats des sources anglaises, ukrainiennes et chinoises.
- 🎯 **Priorisation intelligente** — Les résultats sont automatiquement priorisés selon le système d'écriture de la requête (cyrillique, chinois ou latin).
- 🏗️ **Architecture améliorée** — Refonte vers un système basé sur les fournisseurs pour une meilleure extensibilité.
- 🩹 **Meilleure fiabilité** — Gestion des erreurs améliorée garantissant qu'un fournisseur défaillant ne casse pas toute la recherche.
- 📦 **Dépendances** — Ajout de `HtmlAgilityPack` pour une analyse HTML robuste des résultats ukrainiens et chinois.

## 🆕 Nouveautés (v1.2.1)

- ⚙️ **Paramètres entièrement configurables** — Système de configuration basé sur JSON avec mises à jour à l'exécution :
  - `config.json` avec 11 paramètres personnalisables
  - Bascule d'affichage des synonymes, antonymes et exemples
  - Configuration de la taille du cache, des délais d'expiration et des limites de résultats
  - Activation/désactivation de la lecture audio et des opérations de presse-papiers
  - Rechargement automatique des paramètres sans redémarrage
- 🔄 **Logique robuste de nouvelle tentative réseau** — Fiabilité améliorée pour les appels API :
  - Temporisation exponentielle avec conditions intelligentes de réessai
  - Gestion élégante des erreurs réseau transitoires
  - Tentatives et délais de réessai configurables
- 🛠️ **Opérations de presse-papiers améliorées** — Meilleure gestion des threads et fiabilité :
  - Planificateur de tâches STA personnalisé pour la sécurité des threads
  - Gestion des erreurs et protection contre les délais d'expiration améliorées
  - Activation/désactivation configurable des opérations de presse-papiers
- 🔧 **Correction de bug de configuration** — Les paramètres fonctionnent désormais réellement :
  - Correction du problème où les modifications de config.json étaient ignorées
  - Toutes les options de configuration sont désormais correctement prises en compte
  - Rechargement dynamique garantissant un effet immédiat
- 📊 **Débogage amélioré** — Meilleures capacités de dépannage :
  - Option de journalisation détaillée pour des diagnostics précis
  - Rapports d'erreurs améliorés dans tout le plugin
  - Meilleure catégorisation des erreurs réseau

---

<div align="center">
  <sub>Fait avec ❤️ par <a href="https://github.com/ruslanlap">ruslanlap</a></sub>
</div>
