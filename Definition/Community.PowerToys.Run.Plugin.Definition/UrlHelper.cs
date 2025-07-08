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

                LogHelper.WriteLog($"Opening URL: {url}");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError($"OpenUrl Error for {url}: {ex}");
                return false;
            }
        }
    }
} 