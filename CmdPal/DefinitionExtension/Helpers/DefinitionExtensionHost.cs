// Copyright (c) ruslanlap
// Licensed under the MIT license.

using Microsoft.CommandPalette.Extensions;

namespace DefinitionExtension.Helpers;

public static class DefinitionExtensionHost
{
    public static IExtensionHost? Instance { get; private set; }

    public static void Register(IExtensionHost host)
    {
        Instance = host;
    }
}
