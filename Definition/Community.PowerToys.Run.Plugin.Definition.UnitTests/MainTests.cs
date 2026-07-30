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
    }
}
