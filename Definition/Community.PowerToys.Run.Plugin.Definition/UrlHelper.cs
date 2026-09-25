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
                if (!IsHttpUrl(url)) return false;

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

        public static bool IsHttpUrl(string url) =>
            Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
