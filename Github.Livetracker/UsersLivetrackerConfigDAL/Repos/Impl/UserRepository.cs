using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<int> AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<int> AddKeywordForUserAsync(string keyword, int userId)
        {
            User user = await _dbContext.Users.Where(u => u.UserId == userId)
                .Include(u => u.Keywords).FirstOrDefaultAsync();

            if (user == null || user.Keywords.Any(keyword => keyword.Equals(keyword)))
                return 0;

            Keyword dbKeyword = await
                        _dbContext.Keywords.FirstOrDefaultAsync(keyword => keyword.Word.Equals(keyword));

            if (dbKeyword != null)
                user.Keywords.Add(dbKeyword);
            else
                user.Keywords.Add(new Keyword { Word = keyword });

            return await _dbContext.SaveChangesAsync();
        }

        public Task<User> GetUserByHashedTokenAsync(string hashedToken)
        {
            return _dbContext.Users.Where(u => u.TokenHash.Equals(hashedToken))
                .Include(u => u.Keywords)
                .FirstOrDefaultAsync();
        }

        public IAsyncEnumerable<Keyword> GetAllUserKeywords(int userId)
        {
            return _dbContext.Users.Where(u => u.UserId == userId)
                .Include(u => u.Keywords)
                .SelectMany(u => u.Keywords)
                .AsAsyncEnumerable();
        }
    }
}
