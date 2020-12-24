using System;
using System.Security.Principal;

namespace SettingsProxyAPI.Auth
{
    public class UserIdentity : IIdentity
    {
        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }

        public string Name { get; }

        public UserIdentity(string authenticationType, int userId)
        {
            if(string.IsNullOrWhiteSpace(authenticationType))
                throw new ArgumentNullException(nameof(authenticationType));

            AuthenticationType = authenticationType;
            Name = userId.ToString();
            IsAuthenticated = true;
        }
    }
}
