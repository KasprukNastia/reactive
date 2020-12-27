using System;
using UsersLivetrackerConfigDAL.Models;

namespace SettingsProxyAPI.Auth.WebSockets
{
    public interface IWebSocketsAuthHandler
    {
        IObservable<User> IdentifyUser(string accessToken);
    }
}
