using ManagedCommon;
using Wox.Plugin;
using System.Diagnostics;
using System.IO;


namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class IconManager
    {
        public string DefinitionIcon { get; private set; }
        public string ExampleIcon { get; private set; }
        public string SynonymIcon { get; private set; }
        public string AntonymIcon { get; private set; }
        public string ErrorIcon { get; private set; }
        public string InfoIcon { get; private set; }

        private string _pluginDirectory;

        public void Initialize(string pluginDirectory, Theme theme)
        {
            _pluginDirectory = pluginDirectory;
            UpdateTheme(theme);
        }

        public void UpdateTheme(Theme theme)
        {
            var isLightTheme = theme == Theme.Light || theme == Theme.HighContrastWhite;
            var themePrefix = isLightTheme ? "light" : "dark";

            DefinitionIcon = GetIconPath($"definition.{themePrefix}.png");
            ExampleIcon = GetIconPath($"example.{themePrefix}.png");
            SynonymIcon = GetIconPath($"synonym.{themePrefix}.png");
            AntonymIcon = GetIconPath($"antonym.{themePrefix}.png");
            ErrorIcon = GetIconPath($"error.{themePrefix}.png");
            InfoIcon = GetIconPath($"info.{themePrefix}.png");

            Debug.WriteLine($"[Definition Plugin] Icons updated for theme: {theme}");
        }

        private string GetIconPath(string filename) => Path.Combine(_pluginDirectory, "Images", filename);
    }
} 