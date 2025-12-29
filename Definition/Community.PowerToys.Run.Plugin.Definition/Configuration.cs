using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Diagnostics;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class PluginConfiguration
    {
        public int CacheMaxSize { get; set; } = 100;
        public int HttpTimeoutSeconds { get; set; } = 10;
        public int CacheExpirationMinutes { get; set; } = 30;
        public bool EnableAudioPlayback { get; set; } = true;
        public bool EnableClipboardOperations { get; set; } = true;
        public int TextTruncateLength { get; set; } = 30;
        public bool EnableVerboseLogging { get; set; } = false;
        public string ApiEndpoint { get; set; } = "https://api.dictionaryapi.dev/api/v2/entries/en/";
        public int MaxResultsPerMeaning { get; set; } = 3;
        public bool ShowExamplesInResults { get; set; } = true;
        public bool ShowSynonymsInResults { get; set; } = true;
        public bool ShowAntonymsInResults { get; set; } = true;
        public string Language { get; set; } = "en";
        public string UkrainianApiEndpoint { get; set; } = "https://sum.in.ua/s/";
    }

    internal static class ConfigurationManager
    {
        private static readonly string ConfigFilePath;
        private static PluginConfiguration _configuration;

        static ConfigurationManager()
        {
            var pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ConfigFilePath = Path.Combine(pluginDirectory, "config.json");
            LoadConfiguration();
        }

        public static PluginConfiguration Configuration => _configuration ??= new PluginConfiguration();

        private static void LoadConfiguration()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    var jsonContent = File.ReadAllText(ConfigFilePath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        WriteIndented = true
                    };
                    _configuration = JsonSerializer.Deserialize<PluginConfiguration>(jsonContent, options);
                    Debug.WriteLine($"[Definition Plugin] Configuration loaded from {ConfigFilePath}");
                }
                else
                {
                    _configuration = new PluginConfiguration();
                    SaveConfiguration();
                    Debug.WriteLine($"[Definition Plugin] Default configuration created at {ConfigFilePath}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Definition Plugin] Error loading configuration: {ex.Message}");
                _configuration = new PluginConfiguration();
            }
        }

        public static void SaveConfiguration()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };
                var jsonContent = JsonSerializer.Serialize(_configuration, options);
                File.WriteAllText(ConfigFilePath, jsonContent);
                Debug.WriteLine($"[Definition Plugin] Configuration saved to {ConfigFilePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Definition Plugin] Error saving configuration: {ex.Message}");
            }
        }

        public static void ReloadConfiguration()
        {
            LoadConfiguration();
        }

        public static void UpdateConfiguration(Action<PluginConfiguration> updateAction)
        {
            updateAction(_configuration);
            SaveConfiguration();
        }
    }
}