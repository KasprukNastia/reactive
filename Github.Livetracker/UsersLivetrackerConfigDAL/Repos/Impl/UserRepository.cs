using System;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace UsersLivetrackerConfigDAL.Repos.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersLivetrackerContext _dbContext;

        public UserRepository(UsersLivetrackerContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
