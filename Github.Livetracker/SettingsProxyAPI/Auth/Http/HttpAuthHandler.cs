using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.Auth.Http
{
    public class HttpAuthHandler : AuthenticationHandler<HttpAuthScheme>
    {
        internal const string InternalAuthHeaderKey = "Authentication";
        public static string AuthenticationHeaderKey => InternalAuthHeaderKey;

        private readonly IUserRepository _userRepository;

        public HttpAuthHandler(
            IUserRepository userRepository,
            IOptionsMonitor<HttpAuthScheme> options,
            ILoggerFactory logger, 
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(AuthenticationHeaderKey, out StringValues authHeader) || !authHeader.Any())
                return AuthenticateResult.NoResult();

            string authToken = authHeader.First();
            if (string.IsNullOrWhiteSpace(authToken))
                return AuthenticateResult.Fail("Authentication header is an empty string");

            string hashedToken = authToken.GetTokenHash();

            User user = await _userRepository.GetUserByHashedTokenAsync(hashedToken);
            if (user == null)
                return AuthenticateResult.Fail("User with such token was not found");

            var ticket = new AuthenticationTicket(
                principal: new ClaimsPrincipal(new UserIdentity("SettingsAuth", user.Id)),
                authenticationScheme: Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
