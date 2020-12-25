using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;

namespace UsersLivetrackerConfigDAL.Repos.Interfaces
{
    public interface IKeywordRepository
    {
        bool TryAddKeyword(Keyword keyword);
    }
}
