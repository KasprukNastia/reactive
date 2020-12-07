using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;

namespace UsersLivetrackerConfigDAL.Repos.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddUserAsync(User user);
    }
}
