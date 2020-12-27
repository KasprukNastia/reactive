using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.Auth.WebSockets
{
    public class WebSocketsAuthHandler : IWebSocketsAuthHandler
    {
        private readonly IUserRepository _userRepository;

        public WebSocketsAuthHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public IObservable<User> IdentifyUser(HttpContext context)
        {
            return Observable
                .Create<User>(async observer =>
                {
                    string accessToken = context.Request.Query["access_token"];

                    if (string.IsNullOrWhiteSpace(accessToken))
                        throw new UnauthorizedAccessException("Access token is an empty string");

                    string hashedToken = accessToken.GetTokenHash();

                    User user = await _userRepository.GetUserByHashedTokenAsync(hashedToken);
                    if(user == null)
                        throw new UnauthorizedAccessException("User with such token was not found");
                    observer.OnNext(user);
                })
                .Do(
                    onNext: user => context.User.AddIdentity(new ClaimsIdentity(new UserIdentity("SettingsAuth", user.Id))),
                    onError: async exception => 
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        byte[] output = Encoding.UTF8.GetBytes(exception.Message);
                        await webSocket.SendAsync(new ArraySegment<byte>(output, 0, output.Length),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                    });
                
        }
    }
}
