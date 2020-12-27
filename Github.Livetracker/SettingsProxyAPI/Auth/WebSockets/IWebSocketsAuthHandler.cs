using Microsoft.AspNetCore.Http;
using System;
using UsersLivetrackerConfigDAL.Models;

namespace SettingsProxyAPI.Auth.WebSockets
{
    public interface IWebSocketsAuthHandler
    {
        IObservable<User> IdentifyUser(HttpContext context);
    }
}
