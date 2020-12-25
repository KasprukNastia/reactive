using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAPI.Tokens
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken(string userName) =>
            string.Join('-', userName, Guid.NewGuid());
    }
}
