using AuthAPI.AppCode.Interfaces;
using NSec.Cryptography;
using System;
using System.Text;

namespace AuthAPI.AppCode.Impl
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
