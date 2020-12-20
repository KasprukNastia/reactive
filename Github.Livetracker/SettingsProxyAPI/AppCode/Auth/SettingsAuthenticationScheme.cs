using Microsoft.AspNetCore.Authentication;

namespace SettingsProxyAPI.AppCode.Auth
{
    public class SettingsAuthenticationScheme : AuthenticationSchemeOptions
    {
        public static string SchemeName => "SettingsAuthenticationScheme";
    }
}
