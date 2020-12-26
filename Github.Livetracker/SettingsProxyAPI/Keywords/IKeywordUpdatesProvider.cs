using SettingsProxyAPI.Models;
using System;

namespace SettingsProxyAPI.Keywords
{
    public interface IKeywordUpdatesProvider
    {
        IObservable<KeywordOutput> GetKeywordSequence(KeywordInput keywordInput);
        bool RemoveKeywordSequence(KeywordInput keywordInput);
    }
}
