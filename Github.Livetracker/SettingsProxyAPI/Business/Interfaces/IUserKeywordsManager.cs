using SettingsProxyAPI.AppCode.Auth;
using SettingsProxyAPI.Models;
using System;
using System.Threading.Tasks;

namespace SettingsProxyAPI.Business.Interfaces
{
    public interface IUserKeywordsManager
    {
        Task<IObservable<KeywordInfo>> OnUserConnected(int userId);
        Task<IObservable<KeywordInfo>> AppendKeyword(IObservable<KeywordInfo> keywordsObservable, string keyword);
    }
}
