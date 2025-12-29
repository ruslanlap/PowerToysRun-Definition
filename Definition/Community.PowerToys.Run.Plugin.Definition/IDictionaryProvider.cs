using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal interface IDictionaryProvider
    {
        Task<List<DictionaryEntry>> LookupAsync(string word, CancellationToken token);
        string LanguageCode { get; }
        string DisplayName { get; }
    }
}
