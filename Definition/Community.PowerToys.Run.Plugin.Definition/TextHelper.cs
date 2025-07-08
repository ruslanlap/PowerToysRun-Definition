namespace Community.PowerToys.Run.Plugin.Definition
{
    internal static class TextHelper
    {
        public static string Truncate(string text, int? maxLength = null)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var length = maxLength ?? ConfigurationManager.Configuration.TextTruncateLength;
            return text.Length <= length ? text : text.Substring(0, length) + "...";
        }
    }
} 