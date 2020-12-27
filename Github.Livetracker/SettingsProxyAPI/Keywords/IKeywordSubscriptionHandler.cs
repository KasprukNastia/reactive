using SettingsProxyAPI.Models;
using System;
using UsersLivetrackerConfigDAL.Models;

namespace SettingsProxyAPI.Keywords
{
    public interface IKeywordSubscriptionHandler
    {
        IObservable<string> Handle(User user, IObservable<KeywordSubscriptionRequest> userKeywordRequests);
    }
}
