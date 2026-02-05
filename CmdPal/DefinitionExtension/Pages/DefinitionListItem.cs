// Copyright (c) ruslanlap
// Licensed under the MIT license.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Diagnostics;

namespace DefinitionExtension.Pages;

public enum DefinitionItemType
{
    Definition,
    Example,
    Synonym,
    Antonym,
    Phonetic,
    WordHeader
}

internal sealed class DefinitionListItem : ListItem
{
    public DefinitionItemType ItemType { get; }
    public string? TextToCopy { get; }
    public string? AudioUrl { get; }
    public string? SourceUrl { get; }
    public string? Word { get; }

    public DefinitionListItem(
        string title,
        string subtitle,
        DefinitionItemType itemType,
        string? textToCopy = null,
        string? audioUrl = null,
        string? sourceUrl = null,
        string? word = null,
        IconInfo? icon = null,
        Tag[]? tags = null)
        : base(new CopyTextCommand(textToCopy ?? subtitle))
    {
        Title = title;
        Subtitle = subtitle;
        ItemType = itemType;
        TextToCopy = textToCopy ?? subtitle;
        AudioUrl = audioUrl;
        SourceUrl = sourceUrl;
        Word = word;
        Icon = icon ?? GetDefaultIcon(itemType);

        if (tags != null)
        {
            Tags = tags;
        }

        var moreCommands = BuildContextCommands();
        if (moreCommands.Length > 0)
        {
            MoreCommands = moreCommands;
        }
    }

    private static IconInfo GetDefaultIcon(DefinitionItemType type) => type switch
    {
        DefinitionItemType.Definition => new IconInfo("\uE8BD"),  // Book icon
        DefinitionItemType.Example => new IconInfo("\uE8C1"),     // Quote icon
        DefinitionItemType.Synonym => new IconInfo("\uE8D1"),     // Link icon
        DefinitionItemType.Antonym => new IconInfo("\uE7BA"),     // Switch icon
        DefinitionItemType.Phonetic => new IconInfo("\uE767"),    // Volume icon
        DefinitionItemType.WordHeader => new IconInfo("\uE82D"),  // Dictionary icon
        _ => new IconInfo("\uE82D"),
    };

    private IContextItem[] BuildContextCommands()
    {
        var commands = new System.Collections.Generic.List<IContextItem>();

        // Copy to clipboard
        if (!string.IsNullOrEmpty(TextToCopy))
        {
            commands.Add(new CommandContextItem(
                new CopyTextCommand(TextToCopy))
            {
                Title = "Copy to clipboard",
                Icon = new IconInfo("\uE8C8"),
            });
        }

        // Open in Wiktionary
        if (!string.IsNullOrEmpty(Word))
        {
            commands.Add(new CommandContextItem(
                new OpenUrlCommand($"https://en.wiktionary.org/wiki/{Uri.EscapeDataString(Word)}"))
            {
                Title = "Open in Wiktionary",
                Icon = new IconInfo("\uE774"),
            });
        }

        // Open source URL
        if (!string.IsNullOrEmpty(SourceUrl))
        {
            commands.Add(new CommandContextItem(
                new OpenUrlCommand(SourceUrl))
            {
                Title = "Open source",
                Icon = new IconInfo("\uE71B"),
            });
        }

        return commands.ToArray();
    }
}

internal sealed class CopyTextCommand : InvokableCommand
{
    private readonly string _text;

    public CopyTextCommand(string text)
    {
        _text = text ?? string.Empty;
        Name = "Copy";
        Icon = new IconInfo("\uE8C8");
    }

    public override ICommandResult Invoke()
    {
        if (!string.IsNullOrEmpty(_text))
        {
            ClipboardHelper.SetText(_text);
        }
        return CommandResult.KeepOpen();
    }
}

internal sealed class OpenUrlCommand : InvokableCommand
{
    private readonly string _url;

    public OpenUrlCommand(string url)
    {
        _url = url;
        Name = "Open";
        Icon = new IconInfo("\uE774");
    }

    public override ICommandResult Invoke()
    {
        try
        {
            if (Uri.IsWellFormedUriString(_url, UriKind.Absolute))
            {
                Process.Start(new ProcessStartInfo(_url) { UseShellExecute = true });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Definition CmdPal] Failed to open URL: {ex.Message}");
        }
        return CommandResult.Dismiss();
    }
}

internal static class ClipboardHelper
{
    public static void SetText(string text)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-command \"Set-Clipboard -Value '{text.Replace("'", "''")}'\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                }
            };
            process.Start();
            process.WaitForExit(2000);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Definition CmdPal] Clipboard error: {ex.Message}");
        }
    }
}
