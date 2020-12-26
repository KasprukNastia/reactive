using SettingsProxyAPI.Models;
using System;

namespace SettingsProxyAPI.Keywords
{
    public interface IKeywordProvider
    {
        IObservable<KeywordOutput> GetListKeywords(int userId, IObservable<KeywordInput> keywords);
        IObservable<KeywordOutput> GetOneKeyword(int userId, KeywordInput keywordInput);
        IObservable<KeywordOutput> RemoveKeywordForUser(
            IObservable<KeywordOutput> userKeywordsObservable,
            int userId,
            KeywordInput keywordInput);
    }
}
