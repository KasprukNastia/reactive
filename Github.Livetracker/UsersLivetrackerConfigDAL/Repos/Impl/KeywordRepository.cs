using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace UsersLivetrackerConfigDAL.Repos.Impl
{
    public class KeywordRepository : IKeywordRepository, IDisposable
    {
        private readonly IServiceScope _serviceScope;
        private readonly UsersLivetrackerContext _dbContext;

        public KeywordRepository(IServiceScopeFactory scopeFactory)
        {
            _serviceScope = scopeFactory.CreateScope();
            _dbContext = _serviceScope.ServiceProvider.GetService<UsersLivetrackerContext>();
        }

        public async Task<bool> TryAddKeywordAsync(Keyword keyword)
        {
            if (!_dbContext.Keywords.Any(k => k.Word.Equals(keyword)))
                return false;

            _dbContext.Keywords.Add(keyword);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        #region IDisposable

        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        #endregion
    }
}
