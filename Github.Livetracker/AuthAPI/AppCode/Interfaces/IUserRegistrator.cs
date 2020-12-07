using AuthAPI.Models;
using System.Threading.Tasks;

namespace AuthAPI.AppCode.Interfaces
{
    public interface IUserRegistrator
    {
        Task<RegisteredUser> RegisterUserAsync(NewUser newUser);
    }
}
