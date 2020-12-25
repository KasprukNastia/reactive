using AuthAPI.Models;
using AuthAPI.Tokens;
using System;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace AuthAPI.Users
{
    public class UserRegistrator : IUserRegistrator
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokenHasher _tokenHasher;

        public UserRegistrator(
            IUserRepository userRepository, 
            ITokenGenerator tokenGenerator,
            ITokenHasher tokenHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _tokenHasher = tokenHasher ?? throw new ArgumentNullException(nameof(tokenHasher));
        }

        public async Task<RegisteredUser> RegisterUserAsync(NewUser newUser)
        {
            var registeredUser = new RegisteredUser
            {
                Name = newUser.Name,
                Token = _tokenGenerator.GenerateToken(newUser.Name)
            };

            await _userRepository.AddUserAsync(
                new User
                {
                    Name = newUser.Name,
                    TokenHash = _tokenHasher.HashToken(registeredUser.Token)
                });

            return registeredUser;
        }
    }
}
