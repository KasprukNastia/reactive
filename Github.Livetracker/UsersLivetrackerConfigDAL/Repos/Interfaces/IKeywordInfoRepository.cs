using System.Collections.Generic;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;

namespace UsersLivetrackerConfigDAL.Repos.Interfaces
{
    public interface IKeywordInfoRepository
    {
        Task<int> SetRecordsProcessed(List<int> keywordIds);
        IAsyncEnumerable<KeywordInfo> GetAllUnprocessedRecords(string word, string source);
        Task<List<KeywordBySourceItem>> GetKeywordBySourceStatistics(string word);
    }
}
