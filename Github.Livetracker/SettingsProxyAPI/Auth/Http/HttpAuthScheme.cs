using Microsoft.AspNetCore.Authentication;

namespace SettingsProxyAPI.Auth.Http
{
    public class HttpAuthScheme : AuthenticationSchemeOptions
    {
        public static string SchemeName => "SettingsAuthenticationScheme";
    }
}
