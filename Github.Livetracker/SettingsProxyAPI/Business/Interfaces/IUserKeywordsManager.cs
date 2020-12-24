using SettingsProxyAPI.Models;
using System;

namespace SettingsProxyAPI.Business.Interfaces
{
    public interface IUserKeywordsManager
    {
        IObservable<KeywordInfo> OnUserConnected(int userId);
    }
}
