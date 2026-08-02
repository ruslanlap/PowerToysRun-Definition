# Changelog

All notable changes to this project are documented in this file.

## [1.5.4](https://github.com/ruslanlap/PowerToysRun-Definition/compare/v1.5.3...v1.5.4) (2026-08-02)


### Bug Fixes

* remove dead code, deduplicate ValidSubcommands — v1.5.4 ([ffcbaa3](https://github.com/ruslanlap/PowerToysRun-Definition/commit/ffcbaa3cd8950720aceda32287bb46c03312ca6e))
* remove dead code, deduplicate ValidSubcommands ([#13](https://github.com/ruslanlap/PowerToysRun-Definition/issues/13)) ([6972d68](https://github.com/ruslanlap/PowerToysRun-Definition/commit/6972d68ab9ae324c5a96293ef0666a71e57c1f9e))
* remove empty snyk.yml causing CI failures ([a6948a3](https://github.com/ruslanlap/PowerToysRun-Definition/commit/a6948a38fa1e2e326ccf3af5285caad59fee4de7))

## [1.5.3] - 2026-07-31

### Fixed
- Return dedicated pronunciation results instead of matching definition titles that contain phonetics.
- Collect synonyms and antonyms from both meaning-level and definition-level API fields.
- Honor explicit pron, syn, ant, and ex commands for arbitrary words, even when their default result category is hidden.
- Show a category-specific message when the dictionary has no requested data.
- Add regression coverage for every long and short subcommand alias.

## [1.5.2] - 2026-07-30

### Fixed
- Apply parsed subcommands when filtering delayed dictionary results.
- Keep subcommand result caches separate from default word lookups.
- Bump plugin metadata and assembly versions to 1.5.2.

## [1.5.0] - 2026-06-30

### Added
- Italian dictionary provider backed by Wikizionario (`it.wiktionary.org`).
- Italian provider registration under language code `it`.

### Changed
- Default `LatinLanguages` configuration from `"en,fr"` to `"en,fr,it"`.
- README download links, feature list, configuration docs, and release notes for v1.5.0.

## [1.4.0] - Previous release

### Added
- French dictionary support via Collins French-English dictionary with Wiktionnaire fallback.
- Automatic language detection and multi-language Latin lookups.
- `LatinLanguages` configuration setting.

[1.5.3]: https://github.com/ruslanlap/PowerToysRun-Definition/releases/tag/v1.5.3
[1.5.2]: https://github.com/ruslanlap/PowerToysRun-Definition/releases/tag/v1.5.2
[1.5.0]: https://github.com/ruslanlap/PowerToysRun-Definition/releases/tag/v1.5.0
[1.4.0]: https://github.com/ruslanlap/PowerToysRun-Definition/releases/tag/v1.4.0
