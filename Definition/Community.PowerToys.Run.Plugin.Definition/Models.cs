using System;
using System.Collections.Generic;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class DictionaryEntry
    {
        public string Word { get; set; }
        public string Phonetic { get; set; }
        public List<Phonetic> Phonetics { get; set; } = new();
        public List<Meaning> Meanings { get; set; } = new();
        public LicenseInfo License { get; set; }
        public List<string> SourceUrls { get; set; } = new();
    }

    internal class Phonetic
    {
        public string Text { get; set; }
        public string Audio { get; set; }
        public string SourceUrl { get; set; }
        public LicenseInfo License { get; set; }
    }

    internal class Meaning
    {
        public string PartOfSpeech { get; set; }
        public List<DefinitionItem> Definitions { get; set; } = new();
        public List<string> Synonyms { get; set; } = new();
        public List<string> Antonyms { get; set; } = new();
    }

    internal class DefinitionItem
    {
        public string Definition { get; set; }
        public string Example { get; set; }
    }

    internal class LicenseInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    internal class ResultContext
    {
        public string TextToCopy { get; set; }
        public string AudioUrl { get; set; }
        public string SourceUrl { get; set; }
        public string Word { get; set; }
    }

    internal class CacheItem
    {
        public List<Result> Results { get; }
        public DateTime Timestamp { get; }

        public CacheItem(List<Result> results, DateTime timestamp)
        {
            Results = results;
            Timestamp = timestamp;
        }

        public bool IsExpired => DateTime.UtcNow - Timestamp > TimeSpan.FromMinutes(30);
    }

    internal static class ResultExtensions
    {
        public static Result Clone(this Result result, string newQueryTextDisplay)
        {
            return new Result
            {
                QueryTextDisplay = newQueryTextDisplay,
                IcoPath = result.IcoPath,
                Title = result.Title,
                SubTitle = result.SubTitle,
                ToolTipData = result.ToolTipData,
                Action = result.Action,
                ContextData = result.ContextData,
                Score = result.Score
            };
        }
    }
} 