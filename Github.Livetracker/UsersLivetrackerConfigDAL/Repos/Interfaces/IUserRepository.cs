using System.Collections.Generic;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;

namespace UsersLivetrackerConfigDAL.Repos.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddUserAsync(User user);
        Task<int> AddKeywordForUserAsync(string keyword, int userId);
        Task<User> GetUserByHashedTokenAsync(string hashedToken);
        IAsyncEnumerable<Keyword> GetAllUserKeywords(int userId);
    }
}
