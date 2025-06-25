using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class ResultProcessor
    {
        private readonly IconManager _iconManager;

        public ResultProcessor(IconManager iconManager)
        {
            _iconManager = iconManager;
        }

        public List<Result> ProcessEntry(DictionaryEntry entry, string rawSearch)
        {
            var results = new List<Result>();
            var sourceUrl = entry.SourceUrls?.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
            var phoneticInfo = GetPhoneticInfo(entry);

            var titlePrefix = FormatTitlePrefix(entry.Word, phoneticInfo.Text);

            foreach (var meaning in entry.Meanings?.Where(m => m != null) ?? Enumerable.Empty<Meaning>())
            {
                var partOfSpeech = meaning.PartOfSpeech ?? "unknown";
                var titleWithPOS = $"{titlePrefix} ({partOfSpeech})";

                results.AddRange(ProcessDefinitions(meaning, titleWithPOS, phoneticInfo.AudioUrl, sourceUrl, entry.Word, rawSearch));
                results.AddRange(ProcessSynonyms(meaning, partOfSpeech, sourceUrl, entry.Word, rawSearch));
                results.AddRange(ProcessAntonyms(meaning, partOfSpeech, sourceUrl, entry.Word, rawSearch));
            }

            return results.Any() 
                ? results 
                : new List<Result> { CreateResult(rawSearch, _iconManager.InfoIcon, "No definitions found", "No processable definitions in API response.", null, 0) };
        }

        private (string Text, string AudioUrl) GetPhoneticInfo(DictionaryEntry entry)
        {
            var firstPhoneticWithText = entry.Phonetics?.FirstOrDefault(p => p != null && !string.IsNullOrWhiteSpace(p.Text));
            var phoneticText = firstPhoneticWithText?.Text ?? entry.Phonetic;
            var audioUrl = entry.Phonetics?.FirstOrDefault(p => p != null && !string.IsNullOrWhiteSpace(p.Audio))?.Audio;

            return (phoneticText, audioUrl);
        }

        private static string FormatTitlePrefix(string word, string phoneticText)
        {
            return string.IsNullOrWhiteSpace(phoneticText) 
                ? word 
                : $"{word} [{phoneticText}]";
        }

        private List<Result> ProcessDefinitions(Meaning meaning, string titleWithPOS, string audioUrl, string sourceUrl, string word, string rawSearch)
        {
            var results = new List<Result>();
            var definitions = meaning.Definitions?.Where(d => d != null && !string.IsNullOrWhiteSpace(d.Definition)) ?? Enumerable.Empty<DefinitionItem>();

            foreach (var definition in definitions)
            {
                var contextData = new ResultContext
                {
                    TextToCopy = definition.Definition,
                    AudioUrl = audioUrl,
                    SourceUrl = sourceUrl,
                    Word = word
                };

                results.Add(CreateResult(rawSearch, _iconManager.DefinitionIcon, titleWithPOS, definition.Definition, contextData, 100));

                if (!string.IsNullOrWhiteSpace(definition.Example))
                {
                    var exampleContext = new ResultContext 
                    { 
                        TextToCopy = definition.Example, 
                        SourceUrl = sourceUrl, 
                        Word = word 
                    };

                    results.Add(CreateResult(rawSearch, _iconManager.ExampleIcon, $"Example ({meaning.PartOfSpeech})", definition.Example, exampleContext, 90));
                }
            }

            return results;
        }

        private List<Result> ProcessSynonyms(Meaning meaning, string partOfSpeech, string sourceUrl, string word, string rawSearch)
        {
            var validSynonyms = meaning.Synonyms?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            if (validSynonyms?.Any() != true) return new List<Result>();

            var synonymsText = string.Join(", ", validSynonyms);
            var contextData = new ResultContext { TextToCopy = synonymsText, SourceUrl = sourceUrl, Word = word };

            return new List<Result> 
            {
                CreateResult(rawSearch, _iconManager.SynonymIcon, $"Synonyms ({partOfSpeech})", synonymsText, contextData, 80)
            };
        }

        private List<Result> ProcessAntonyms(Meaning meaning, string partOfSpeech, string sourceUrl, string word, string rawSearch)
        {
            var validAntonyms = meaning.Antonyms?.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
            if (validAntonyms?.Any() != true) return new List<Result>();

            var antonymsText = string.Join(", ", validAntonyms);
            var contextData = new ResultContext { TextToCopy = antonymsText, SourceUrl = sourceUrl, Word = word };

            return new List<Result> 
            {
                CreateResult(rawSearch, _iconManager.AntonymIcon, $"Antonyms ({partOfSpeech})", antonymsText, contextData, 75)
            };
        }

        private static Result CreateResult(string rawSearch, string iconPath, string title, string subTitle, ResultContext contextData, int score)
        {
            return new Result
            {
                QueryTextDisplay = rawSearch,
                IcoPath = iconPath,
                Title = title,
                SubTitle = subTitle,
                ToolTipData = new ToolTipData(title, subTitle),
                Action = _ => ClipboardHelper.CopyToClipboard(contextData?.TextToCopy),
                ContextData = contextData,
                Score = score
            };
        }
    }
} 