# Repository Guidelines

## Project Structure & Module Organization

The solution is `Definition/Definition.sln`. Plugin code lives in `Definition/Community.PowerToys.Run.Plugin.Definition/`; `Main.cs` handles PowerToys Run integration, while `*DictionaryProvider.cs` files handle language sources. Keep plugin icons in `Images/`, the bundled Chinese dictionary in `Resources/`, and plugin metadata in `plugin.json`. MSTest cases live in the sibling `Community.PowerToys.Run.Plugin.Definition.UnitTests/` project. Root `data/` holds README images and demos; `docs/` holds translated READMEs. Release automation is in `.github/workflows/`.

## Build, Test, and Development Commands

Use the .NET 9 SDK. The plugin targets Windows 10 build 22621 or later and supports x64 and ARM64. From the repository root:

```sh
dotnet build Definition/Definition.sln -p:Platform=x64
dotnet test Definition/Definition.sln -p:Platform=x64
dotnet publish Definition/Community.PowerToys.Run.Plugin.Definition/Community.PowerToys.Run.Plugin.Definition.csproj -c Release -r win-x64 -p:Platform=x64
```

These commands build the solution, run MSTest, and produce a release publish directory, respectively. Use `ARM64` and `win-arm64` for that architecture. Run the plugin locally by placing its published files in `%LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\Definition\`, restarting PowerToys, and entering `def <word>`.

## Coding Style & Naming Conventions

Follow `.editorconfig`: UTF-8, final newlines, no trailing whitespace, four-space indentation for C#, and two spaces for JSON and project XML. C# uses Allman braces, PascalCase types and constants, and `_camelCase` private fields. Keep dictionary-specific behavior in the corresponding provider; use existing helpers before adding new ones.

## Testing Guidelines

Tests use MSTest (`[TestClass]`, `[TestMethod]`, and `[DataTestMethod]`). Name test files after the class under test, as in `SuggestionProviderTests.cs`, and use descriptive method names such as `ParseSubcommand_should_accept_any_word`. Add or update focused tests for changed behavior, then run `dotnet test` as above. No coverage threshold is configured.

## Commit & Pull Request Guidelines

Recent commits commonly use `feat:`, `fix:`, `docs:`, `ci:`, or `chore:` prefixes; release automation also creates `chore(master): release ...` commits. Use a short, imperative subject that explains the change. Open PRs from a feature branch with a concise description, relevant issue link when one exists, and test results. Include screenshots or a short demo for visible PowerToys result or icon changes.
