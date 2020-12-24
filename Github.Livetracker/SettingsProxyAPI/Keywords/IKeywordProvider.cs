using SettingsProxyAPI.Models;
using System;

namespace SettingsProxyAPI.Keywords
{
    public interface IKeywordProvider
    {
        IObservable<KeywordInfo> GetListKeywords(IObservable<string> keywords);
        IObservable<KeywordInfo> GetOneKeyword(string keyword);
    }
}
