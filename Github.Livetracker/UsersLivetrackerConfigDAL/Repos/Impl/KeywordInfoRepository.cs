using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace UsersLivetrackerConfigDAL.Repos.Impl
{
    public class KeywordInfoRepository : IKeywordInfoRepository, IDisposable
    {
        private readonly IServiceScope _serviceScope;
        private readonly UsersLivetrackerContext _dbContext;

        public KeywordInfoRepository(IServiceScopeFactory scopeFactory)
        {
            _serviceScope = scopeFactory.CreateScope();
            _dbContext = _serviceScope.ServiceProvider.GetService<UsersLivetrackerContext>();
        }

        public Task<int> SetKeywordsProcessed(List<int> keywordInfoIds)
        {
            foreach (KeywordInfo keywordInfo in _dbContext.KeywordInfos.Where(k => keywordInfoIds.Contains(k.Id)))
                keywordInfo.WasProcessed = true;

            return _dbContext.SaveChangesAsync();
        }

        public IAsyncEnumerable<KeywordInfo> GetAllUnprocessedKeywords(string word, string source)
        {
            return _dbContext.KeywordInfos
                .Where(k => k.Word.Equals(word) && k.Source.Equals(source) && (!k.WasProcessed.HasValue || !k.WasProcessed.Value))
                .AsAsyncEnumerable();
        }

        #region IDisposable

        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        #endregion
    }
}
