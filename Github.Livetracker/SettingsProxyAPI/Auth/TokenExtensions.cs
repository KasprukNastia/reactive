using NSec.Cryptography;
using System;
using System.Text;

namespace SettingsProxyAPI.Auth
{
    public static class TokenExtensions
    {
        public static string GetTokenHash(this string token)
        {
            HashAlgorithm algorithm = HashAlgorithm.Sha256;
            byte[] hashedTokenBytes = algorithm.Hash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hashedTokenBytes);
        }
    }
}
