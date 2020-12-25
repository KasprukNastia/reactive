using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;

namespace UsersLivetrackerConfigDAL.Repos.Interfaces
{
    public interface IKeywordRepository
    {
        Task<bool> TryAddKeywordAsync(Keyword keyword);
    }
}
