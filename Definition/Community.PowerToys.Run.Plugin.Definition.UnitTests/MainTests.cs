using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Definition.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main main;

        [TestInitialize]
        public void TestInitialize()
        {
            main = new Main();
        }

        [TestMethod]
        public void Query_should_return_results()
        {
            var results = main.Query(new("search"));

            Assert.IsNotNull(results.First());
        }

        [TestMethod]
        public void LoadContextMenus_should_return_results()
        {
            var results = main.LoadContextMenus(new Result { ContextData = "search" });

            Assert.IsNotNull(results.First());
        }

        [TestMethod]
        public void FilterResultsBySubcommand_should_return_only_matching_results()
        {
            var results = new List<Result>
            {
                new Result { Title = "word [wurd]", SubTitle = "A unit of language" },
                new Result { Title = "Synonyms (noun)", SubTitle = "term, expression" },
                new Result { Title = "Antonyms (noun)", SubTitle = "silence" }
            };

            var method = typeof(Main).GetMethod("FilterResultsBySubcommand", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(method);
            var filtered = (List<Result>)method.Invoke(null, new object[] { results, "synonyms" });

            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual("Synonyms (noun)", filtered[0].Title);
        }

        [DataTestMethod]
        [DataRow("pronunciation world", "pronunciation", "world")]
        [DataRow("pron world", "pron", "world")]
        [DataRow("synonyms test", "synonyms", "test")]
        [DataRow("syn test", "syn", "test")]
        [DataRow("antonyms good", "antonyms", "good")]
        [DataRow("ant good", "ant", "good")]
        [DataRow("examples run", "examples", "run")]
        [DataRow("ex run", "ex", "run")]
        public void ParseSubcommand_should_accept_any_word(string query, string expectedSubcommand, string expectedWord)
        {
            var method = typeof(Main).GetMethod("ParseSubcommand", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(method);

            var parsed = ((string Subcommand, string SearchWord))method.Invoke(null, new object[] { query });

            Assert.AreEqual(expectedSubcommand, parsed.Subcommand);
            Assert.AreEqual(expectedWord, parsed.SearchWord);
        }

        [DataTestMethod]
        [DataRow("pron", "Pronunciation: test")]
        [DataRow("syn", "Synonyms (noun)")]
        [DataRow("ant", "Antonyms (noun)")]
        [DataRow("ex", "Example (noun)")]
        public void FilterResultsBySubcommand_should_match_exact_result_category(string subcommand, string expectedTitle)
        {
            var results = new List<Result>
            {
                new Result { Title = "test [/test/] (noun)", SubTitle = "A procedure." },
                new Result { Title = "Pronunciation: test", SubTitle = "/test/" },
                new Result { Title = "Synonyms (noun)", SubTitle = "trial" },
                new Result { Title = "Antonyms (noun)", SubTitle = "recess" },
                new Result { Title = "Example (noun)", SubTitle = "The test passed." }
            };

            var method = typeof(Main).GetMethod("FilterResultsBySubcommand", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(method);
            var filtered = (List<Result>)method.Invoke(null, new object[] { results, subcommand });

            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(expectedTitle, filtered[0].Title);
        }

        [TestMethod]
        public void ProcessEntry_should_include_definition_level_synonyms_and_antonyms()
        {
            var processor = new ResultProcessor(new IconManager());
            var entry = new DictionaryEntry
            {
                Word = "test",
                Meanings = new List<Meaning>
                {
                    new Meaning
                    {
                        PartOfSpeech = "noun",
                        Definitions = new List<DefinitionItem>
                        {
                            new DefinitionItem
                            {
                                Definition = "A procedure intended to establish quality.",
                                Synonyms = new List<string> { "trial" },
                                Antonyms = new List<string> { "guess" }
                            }
                        }
                    }
                }
            };

            var results = processor.ProcessEntry(entry, "syn test");

            Assert.IsTrue(results.Any(r => r.Title == "Synonyms (noun)" && r.SubTitle == "trial"));
            Assert.IsTrue(results.Any(r => r.Title == "Antonyms (noun)" && r.SubTitle == "guess"));
        }

        [TestMethod]
        public void Explicit_subcommands_should_override_general_visibility_settings()
        {
            var configuration = ConfigurationManager.Configuration;
            var originalExamples = configuration.ShowExamplesInResults;
            var originalSynonyms = configuration.ShowSynonymsInResults;
            var originalAntonyms = configuration.ShowAntonymsInResults;

            try
            {
                configuration.ShowExamplesInResults = false;
                configuration.ShowSynonymsInResults = false;
                configuration.ShowAntonymsInResults = false;

                var processor = new ResultProcessor(new IconManager());
                var entry = new DictionaryEntry
                {
                    Word = "test",
                    Meanings = new List<Meaning>
                    {
                        new Meaning
                        {
                            PartOfSpeech = "noun",
                            Synonyms = new List<string> { "trial" },
                            Antonyms = new List<string> { "recess" },
                            Definitions = new List<DefinitionItem>
                            {
                                new DefinitionItem
                                {
                                    Definition = "A procedure intended to establish quality.",
                                    Example = "The test passed."
                                }
                            }
                        }
                    }
                };

                Assert.IsTrue(processor.ProcessEntry(entry, "syn test", "syn").Any(r => r.Title.StartsWith("Synonyms (")));
                Assert.IsTrue(processor.ProcessEntry(entry, "ant test", "ant").Any(r => r.Title.StartsWith("Antonyms (")));
                Assert.IsTrue(processor.ProcessEntry(entry, "ex test", "ex").Any(r => r.Title.StartsWith("Example (")));
            }
            finally
            {
                configuration.ShowExamplesInResults = originalExamples;
                configuration.ShowSynonymsInResults = originalSynonyms;
                configuration.ShowAntonymsInResults = originalAntonyms;
            }
        }

        [TestMethod]
        public void ProcessEntry_should_include_pronunciation_result()
        {
            var processor = new ResultProcessor(new IconManager());
            var entry = new DictionaryEntry
            {
                Word = "hello",
                Phonetic = "huh-loh",
                Meanings = new List<Meaning>
                {
                    new Meaning
                    {
                        PartOfSpeech = "exclamation",
                        Definitions = new List<DefinitionItem>
                        {
                            new DefinitionItem { Definition = "Used as a greeting." }
                        }
                    }
                }
            };

            var results = processor.ProcessEntry(entry, "pron hello");

            Assert.IsTrue(results.Any(r => r.Title == "Pronunciation: hello" && r.SubTitle == "huh-loh"));
        }
    }
}
