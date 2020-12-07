using AuthAPI.AppCode.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAPI.AppCode.Impl
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly string _secret;

		public TokenGenerator(string secret)
        {
            _secret = secret ?? throw new ArgumentNullException(nameof(secret));
        }

        public string GenerateToken(string userName)
        {
			var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret));

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, userName),
				}),
				SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.Aes256CbcHmacSha512)
			};

			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
    }
}
