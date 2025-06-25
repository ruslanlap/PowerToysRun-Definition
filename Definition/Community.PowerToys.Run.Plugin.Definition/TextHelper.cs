namespace Community.PowerToys.Run.Plugin.Definition
{
    internal static class TextHelper
    {
        public static string Truncate(string text, int maxLength = 30)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }
    }
} 