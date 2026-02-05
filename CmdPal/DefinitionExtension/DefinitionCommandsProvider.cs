// Copyright (c) ruslanlap
// Licensed under the MIT license.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using DefinitionExtension.Helpers;
using DefinitionExtension.Pages;

namespace DefinitionExtension;

public partial class DefinitionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly IconInfo _logoIcon = new("\uE82D"); // Dictionary icon
    private static readonly SettingsManager _settings = new();
    private static readonly DictionaryService _dictionaryService = new();

    public DefinitionCommandsProvider()
    {
        DisplayName = "Definition";
        Settings = _settings.Settings;
        Icon = _logoIcon;

        var settingsPage = _settings.Settings.SettingsPage;

        _commands =
        [
            new CommandItem(new DefinitionPage(_settings, _dictionaryService))
            {
                Title = DisplayName,
                Subtitle = "Look up word definitions, phonetics, synonyms & antonyms",
                Icon = _logoIcon,
                MoreCommands =
                [
                    new CommandContextItem(settingsPage)
                    {
                        Title = "Settings",
                        Subtitle = "Configure the definition lookup settings",
                    },
                ],
            },
        ];
    }

    public override void InitializeWithHost(IExtensionHost host)
    {
        DefinitionExtensionHost.Register(host);
        base.InitializeWithHost(host);
    }

    public override ICommandItem[] TopLevelCommands() => _commands;

    public override IFallbackCommandItem[] FallbackCommands() => [];
}
