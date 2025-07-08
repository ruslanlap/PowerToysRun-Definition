using System;
using System.Diagnostics;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal static class UrlHelper
    {
        public static bool OpenUrl(string url)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) return false;

                Debug.WriteLine($"[Definition Plugin] Opening URL: {url}");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Definition Plugin] OpenUrl Error for {url}: {ex}");
                return false;
            }
        }
    }
} 