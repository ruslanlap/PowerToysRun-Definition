using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Community.PowerToys.Run.Plugin.Definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Community.PowerToys.Run.Plugin.Definition.UnitTests
{
    [TestClass]
    public class SuggestionProviderTests
    {
        private sealed class FakeHandler : HttpMessageHandler
        {
            private readonly HttpStatusCode _code;
            private readonly string _json;

            public FakeHandler(HttpStatusCode code, string json)
            {
                _code = code;
                _json = json;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
            {
                return Task.FromResult(new HttpResponseMessage(_code) { Content = new StringContent(_json, Encoding.UTF8, "application/json") });
            }
        }

        private static SuggestionProvider Make(HttpStatusCode code, string json)
        {
            return new SuggestionProvider(new HttpClient(new FakeHandler(code, json)));
        }

        [TestMethod]
        public async Task ParsesWordsAndSkipsOriginal()
        {
            var provider = Make(HttpStatusCode.OK, "[{\"word\":\"run\"},{\"word\":\"serendipiti\"},{\"word\":\"run\"}]");
            var result = await provider.GetSuggestionsAsync("serendipityy", CancellationToken.None);

            CollectionAssert.AreEqual(new[] { "run", "serendipiti" }, result.ToList());
        }

        [TestMethod]
        public async Task EmptyOnHttpError()
        {
            var provider = Make(HttpStatusCode.NotFound, "[]");
            Assert.AreEqual(0, (await provider.GetSuggestionsAsync("x", CancellationToken.None)).Count);
        }

        [TestMethod]
        public async Task EmptyOnGarbageJson()
        {
            var provider = Make(HttpStatusCode.OK, "<html>not json</html>");
            Assert.AreEqual(0, (await provider.GetSuggestionsAsync("x", CancellationToken.None)).Count);
        }

        [TestMethod]
        public async Task EmptyOnTooLongWord()
        {
            var provider = Make(HttpStatusCode.OK, "[]");
            Assert.AreEqual(0, (await provider.GetSuggestionsAsync(new string('a', 51), CancellationToken.None)).Count);
        }
    }
}
