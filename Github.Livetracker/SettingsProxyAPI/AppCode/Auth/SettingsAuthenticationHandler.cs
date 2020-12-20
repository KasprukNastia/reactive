using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSec.Cryptography;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.AppCode.Auth
{
    public class SettingsAuthenticationHandler : AuthenticationHandler<SettingsAuthenticationScheme>
    {
        internal const string InternalAuthHeaderKey = "Authentication";
        public static string AuthenticationHeaderKey => InternalAuthHeaderKey;

        private readonly IUserRepository _userRepository;

        public SettingsAuthenticationHandler(
            IUserRepository userRepository,
            IOptionsMonitor<SettingsAuthenticationScheme> options, 
            ILoggerFactory logger, UrlEncoder encoder, 
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
            
            HashAlgorithm algorithm = HashAlgorithm.Sha256;
            byte[] hashedTokenBytes = algorithm.Hash(Encoding.UTF8.GetBytes(authToken));
            string hashedToken = Convert.ToBase64String(hashedTokenBytes);

            User user = await _userRepository.GetUserByHashedTokenAsync(hashedToken);
            if(user == null)
                return AuthenticateResult.Fail("User with such token was not found");

            var ticket = new AuthenticationTicket(
                principal: new ClaimsPrincipal(new UserIdentity("SettingsAuth", user.UserId)),
                authenticationScheme: Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
