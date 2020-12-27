using System;
using System.Reactive.Linq;
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

        public IObservable<User> IdentifyUser(string accessToken)
        {
            return Observable
                .Create<User>(async observer =>
                {
                    if (string.IsNullOrWhiteSpace(accessToken))
                        throw new UnauthorizedAccessException("Access token is an empty string");

                    string hashedToken = accessToken.GetTokenHash();

                    User user = await _userRepository.GetUserByHashedTokenAsync(hashedToken);
                    if (user == null)
                        throw new UnauthorizedAccessException("User with such token was not found");
                    observer.OnNext(user);
                });
        }
    }
}
