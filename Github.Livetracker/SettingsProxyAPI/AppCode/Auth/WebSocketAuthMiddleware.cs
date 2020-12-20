using Microsoft.AspNetCore.Http;
using NSec.Cryptography;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.AppCode.Auth
{
    public class WebSocketAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserRepository _userRepository;

        public WebSocketAuthMiddleware(
            RequestDelegate next,
            IUserRepository userRepository)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                string accessToken = context.Request.Query["access_token"];

                if (string.IsNullOrWhiteSpace(accessToken))
                    throw new UnauthorizedAccessException("Access token is an empty string");

                HashAlgorithm algorithm = HashAlgorithm.Sha256;
                byte[] hashedTokenBytes = algorithm.Hash(Encoding.UTF8.GetBytes(accessToken));
                string hashedToken = Convert.ToBase64String(hashedTokenBytes);

                User user = await _userRepository.GetUserByHashedTokenAsync(hashedToken);
                if (user == null)
                    throw new UnauthorizedAccessException("User with such token was not found");

                context.User.AddIdentity(new ClaimsIdentity(new UserIdentity("SettingsAuth", user.UserId)));

                await _next.Invoke(context);
            }
            else if (_next != null)
            {
                await _next.Invoke(context);
            }
        }
    }
}
