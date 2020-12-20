using SettingsProxyAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SettingsProxyAPI.Business.Interfaces
{
    public interface IKeywordProvider
    {
        Task<IObservable<KeywordInfo>> GetListKeywords(List<string> keywords);
        Task<IObservable<KeywordInfo>> GetOneKeyword(string keyword);
    }
}
