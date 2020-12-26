using System.Collections.Generic;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;

namespace UsersLivetrackerConfigDAL.Repos.Interfaces
{
    public interface IKeywordInfoRepository
    {
        Task<int> SetKeywordsProcessed(List<int> keywordIds);
        IAsyncEnumerable<KeywordInfo> GetAllUnprocessedKeywords(string word, string source);
    }
}
