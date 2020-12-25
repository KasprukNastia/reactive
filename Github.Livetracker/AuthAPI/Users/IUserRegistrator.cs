using AuthAPI.Models;
using System.Threading.Tasks;

namespace AuthAPI.Users
{
    public interface IUserRegistrator
    {
        Task<RegisteredUser> RegisterUserAsync(NewUser newUser);
    }
}
