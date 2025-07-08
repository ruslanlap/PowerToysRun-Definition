using System.Diagnostics;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal static class LogHelper
    {
        public static void WriteLog(string message)
        {
            if (ConfigurationManager.Configuration.EnableVerboseLogging)
            {
                Debug.WriteLine($"[Definition Plugin] {message}");
            }
        }

        public static void WriteError(string message)
        {
            // Always log errors regardless of verbose setting
            Debug.WriteLine($"[Definition Plugin] ERROR: {message}");
        }
    }
} 