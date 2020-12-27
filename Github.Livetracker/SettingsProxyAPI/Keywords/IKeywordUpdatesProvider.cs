using SettingsProxyAPI.Models;
using System;

namespace SettingsProxyAPI.Keywords
{
    public interface IKeywordUpdatesProvider
    {
        IObservable<KeywordOutput> GetKeywordSequence(KeywordRequest keywordInput);
        bool RemoveKeywordSequence(KeywordRequest keywordInput);
    }
}
