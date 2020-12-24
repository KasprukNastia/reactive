using Microsoft.AspNetCore.Http;
using System;
using UsersLivetrackerConfigDAL.Models;

namespace SettingsProxyAPI.Auth
{
    public interface IUserAuthHandler
    {
        IObservable<User> IdentifyUser(HttpContext context);
    }
}
