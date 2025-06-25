using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal static class ClipboardHelper
    {
        public static bool CopyToClipboard(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            try
            {
                var success = false;
                var staThread = new Thread(() =>
                {
                    try
                    {
                        Clipboard.SetDataObject(text, true);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[Definition Plugin] STA Clipboard Error: {ex}");
                    }
                });

                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join(TimeSpan.FromSeconds(5));
                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Definition Plugin] Clipboard Thread Error: {ex}");
                return false;
            }
        }
    }
} 