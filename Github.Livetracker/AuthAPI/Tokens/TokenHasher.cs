using NSec.Cryptography;
using System;
using System.Text;

namespace AuthAPI.Tokens
{
    public class TokenHasher : ITokenHasher
    {
        public string HashToken(string token)
        {
            HashAlgorithm algorithm = HashAlgorithm.Sha256;

            byte[] hashedToken = algorithm.Hash(Encoding.UTF8.GetBytes(token));

            return Convert.ToBase64String(hashedToken);
        }
    }
}
