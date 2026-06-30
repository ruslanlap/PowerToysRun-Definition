# 🔍 PowerToys Run: Definition Plugin (Français)

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

`Definition` est un plugin PowerToys Run qui permet d'obtenir rapidement des définitions, de la phonétique, des exemples, des synonymes et des antonymes avec `def <mot>`.

## 📋 Sommaire

- [Présentation](#-présentation)
- [Fonctionnalités principales](#-fonctionnalités-principales)
- [Langues prises en charge](#-langues-prises-en-charge)
- [Installation](#-installation)
- [Utilisation](#-utilisation)
- [Configuration](#️-configuration)
- [Dépannage rapide](#-dépannage-rapide)
- [Liens utiles](#-liens-utiles)

## 📌 Présentation

Le plugin prend en charge des requêtes naturelles sans préfixe de langue :

- `def world`
- `def enchanté`
- `def amore`
- `def слово`
- `def 你好`

La langue est détectée automatiquement selon l'écriture utilisée :

- alphabet latin → anglais/français/italien (selon `LatinLanguages`)
- alphabet cyrillique → ukrainien
- caractères chinois → chinois

## ✨ Fonctionnalités principales

- Détection automatique de langue via `def <mot>`
- Recherche parallèle sur plusieurs fournisseurs de dictionnaire
- Lecture audio de la prononciation (si disponible)
- Copie rapide des définitions depuis le menu contextuel
- Configuration complète via `config.json`
- Mise en cache des résultats pour accélérer les recherches répétées
- Dictionnaire chinois intégré en mode hors ligne

## 🌐 Langues prises en charge

| Langue | Source | Méthode | Internet |
|---|---|---|:---:|
| **English** | [dictionaryapi.dev](https://dictionaryapi.dev/) | API REST (JSON) | Oui |
| **Français** | [Collins](https://www.collinsdictionary.com/dictionary/french-english/) (principal) + [Wiktionnaire](https://fr.wiktionary.org/) (secours) | HTML + API MediaWiki | Oui |
| **Italiano** | [Wikizionario](https://it.wiktionary.org/) | API MediaWiki | Oui |
| **Українська** | [uk.wiktionary.org](https://uk.wiktionary.org/) (principal) + [goroh.pp.ua](https://goroh.pp.ua/) (secours) | API MediaWiki + HTML | Oui |
| **中文** | Base intégrée CC-CEDICT | Base hors ligne | Non |

## 🚀 Installation


### Installation manuelle

1. Téléchargez l'archive depuis [la dernière version](https://github.com/ruslanlap/PowerToysRun-Definition/releases/latest).
2. Extrayez les fichiers dans :

   ```text
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```

3. Redémarrez PowerToys.
4. Ouvrez PowerToys Run (`Alt + Space`) et testez `def test`.

## 🔧 Utilisation

1. Ouvrez PowerToys Run (`Alt + Space`).
2. Saisissez `def <mot>`.
3. Validez avec <kbd>Enter</kbd>.
4. Menu contextuel disponible sur chaque résultat :
   - copier la définition
   - lire la prononciation
   - ouvrir la source
   - rechercher des mots associés

### Exemples

- `def world`
- `def enchanté`
- `def amore`
- `def слово`
- `def 你好`

## ⚙️ Configuration

Le plugin crée automatiquement `config.json` dans le dossier du plugin.

### Paramètres principaux

| Paramètre | Valeur par défaut | Description |
|---|---|---|
| `Language` | `"en"` | Langue par défaut |
| `LatinLanguages` | `"en,fr,it"` | Langues latines interrogées en parallèle |
| `ApiEndpoint` | `https://api.dictionaryapi.dev/api/v2/entries/en/` | Endpoint API anglais |
| `HttpTimeoutSeconds` | `30` | Délai d'expiration HTTP |
| `CacheMaxSize` | `100` | Taille maximale du cache |
| `CacheExpirationMinutes` | `30` | Durée de vie du cache |
| `EnableAudioPlayback` | `true` | Activer la prononciation audio |
| `EnableClipboardOperations` | `true` | Activer la copie vers le presse-papiers |
| `MaxResultsPerMeaning` | `3` | Nombre max de définitions par sens |
| `ShowExamplesInResults` | `true` | Afficher des exemples |
| `ShowSynonymsInResults` | `true` | Afficher les synonymes |
| `ShowAntonymsInResults` | `true` | Afficher les antonymes |
| `EnableVerboseLogging` | `false` | Activer les logs détaillés |

### Exemple de configuration

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

## 🧪 Dépannage rapide

Si vous n'obtenez aucun résultat :

1. Vérifiez votre connexion Internet (`en`, `fr`, `it`, `uk`).
2. Testez une variante sans accent (`enchante` au lieu de `enchanté`).
3. Vérifiez que le plugin est activé dans `PowerToys Run > Plugins`.
4. Redémarrez PowerToys après une mise à jour du plugin.

## 🔗 Liens utiles

- Documentation complète en anglais : [../README.md](../README.md)
- Releases : [GitHub Releases](https://github.com/ruslanlap/PowerToysRun-Definition/releases)
- Projet : [PowerToysRun-Definition](https://github.com/ruslanlap/PowerToysRun-Definition)
