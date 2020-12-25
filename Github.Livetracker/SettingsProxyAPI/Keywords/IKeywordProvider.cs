using SettingsProxyAPI.Models;
using System;

namespace SettingsProxyAPI.Keywords
{
    public interface IKeywordProvider
    {
        IObservable<KeywordOutput> GetListKeywords(IObservable<string> keywords);
        IObservable<KeywordOutput> GetOneKeyword(string keyword);
    }
}
