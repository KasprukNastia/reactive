using UsersLivetrackerConfigDAL.Models;

namespace UsersLivetrackerConfigDAL.Repos.Interfaces
{
    public interface IKeywordRepository
    {
        (bool addedForUser, bool addedToKeywords) AddKeywordForUser(int userId, string word, string source);
        (bool removedForUser, bool removedFromKeywords) RemoveKeywordForUser(int userId, string word, string source);
    }
}
