using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class ContextMenuBuilder
    {
        private readonly string _pluginName;
        private readonly PluginInitContext _context;
        private readonly AudioManager _audioManager;

        public ContextMenuBuilder(string pluginName, PluginInitContext context, AudioManager audioManager)
        {
            _pluginName = pluginName;
            _context = context;
            _audioManager = audioManager;
        }

        public List<ContextMenuResult> BuildMenuItems(ResultContext context, Result selectedResult)
        {
            var menuItems = new List<ContextMenuResult>();

            AddCopyMenuItem(menuItems, context);
            AddAudioMenuItem(menuItems, context);
            AddSourceUrlMenuItem(menuItems, context);
            AddDefineWordMenuItem(menuItems, context, selectedResult);

            return menuItems;
        }

        private void AddCopyMenuItem(List<ContextMenuResult> menuItems, ResultContext context)
        {
            if (string.IsNullOrWhiteSpace(context.TextToCopy)) return;

            menuItems.Add(new ContextMenuResult
            {
                PluginName = _pluginName,
                Title = $"Copy: {TextHelper.Truncate(context.TextToCopy)} (Ctrl+C)",
                FontFamily = "Segoe MDL2 Assets",
                Glyph = "\xE8C8",
                AcceleratorKey = Key.C,
                AcceleratorModifiers = ModifierKeys.Control,
                Action = _ => ClipboardHelper.CopyToClipboard(context.TextToCopy)
            });
        }

        private void AddAudioMenuItem(List<ContextMenuResult> menuItems, ResultContext context)
        {
            if (!Uri.IsWellFormedUriString(context.AudioUrl, UriKind.Absolute)) return;

            menuItems.Add(new ContextMenuResult
            {
                PluginName = _pluginName,
                Title = "Play Pronunciation",
                FontFamily = "Segoe MDL2 Assets",
                Glyph = "\xE768",
                Action = _ =>
                {
                    try
                    {
                        _audioManager.PlayAudio(context.AudioUrl);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[Definition Plugin] Audio menu action error: {ex}");
                        return false;
                    }
                }
            });
        }

        private void AddSourceUrlMenuItem(List<ContextMenuResult> menuItems, ResultContext context)
        {
            if (!Uri.IsWellFormedUriString(context.SourceUrl, UriKind.Absolute)) return;

            menuItems.Add(new ContextMenuResult
            {
                PluginName = _pluginName,
                Title = "Open Source URL in Browser",
                FontFamily = "Segoe MDL2 Assets",
                Glyph = "\xE774",
                Action = _ => UrlHelper.OpenUrl(context.SourceUrl)
            });
        }

        private void AddDefineWordMenuItem(List<ContextMenuResult> menuItems, ResultContext context, Result selectedResult)
        {
            var isAuxiliaryResult = selectedResult.Title.Contains("Synonyms") ||
                                   selectedResult.Title.Contains("Antonyms") ||
                                   selectedResult.Title.Contains("Example");

            if (!string.IsNullOrWhiteSpace(context.Word) && isAuxiliaryResult)
            {
                menuItems.Add(new ContextMenuResult
                {
                    PluginName = _pluginName,
                    Title = $"Define '{context.Word}'",
                    FontFamily = "Segoe MDL2 Assets",
                    Glyph = "\xE721",
                    Action = _ =>
                    {
                        try
                        {
                            if (_context?.CurrentPluginMetadata != null)
                            {
                                _context.API.ChangeQuery($"{_context.CurrentPluginMetadata.ActionKeyword} {context.Word}", true);
                                return true;
                            }
                            return false;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[Definition Plugin] Define word menu action error: {ex}");
                            return false;
                        }
                    }
                });
            }
        }
    }
} 