using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public (bool addedForUser, bool addedToKeywords) AddKeywordForUser(int userId, string word, string source)
        {
            bool addedForUser = false, addedToKeywords = false;

            Keyword keyword = _dbContext.Keywords.Where(k => k.Word.Equals(word) && k.Source.Equals(source))
                .Include(k => k.Users).FirstOrDefault();
            User user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (keyword == null)
            {
                if (user != null)
                {
                    keyword = new Keyword { Word = word, Source = source, Users = new List<User> { user } };
                    _dbContext.Keywords.Add(keyword);
                    addedForUser = addedToKeywords = _dbContext.SaveChanges() > 0;
                }
            }
            else
            {
                if(user != null && !keyword.Users.Any(u => u.Id == userId))
                {
                    keyword.Users.Add(user);
                    addedForUser = _dbContext.SaveChanges() > 0;
                }
            }

            return(addedForUser, addedToKeywords);
        }

        public (bool removedForUser, bool removedFromKeywords) RemoveKeywordForUser(int userId, string word, string source)
        {
            Keyword keyword = _dbContext.Keywords.Where(k => k.Word.Equals(word) && k.Source.Equals(source))
                .Include(k => k.Users).FirstOrDefault();

            if (keyword == null)
                return (false, false);

            bool removedForUser = keyword.Users.RemoveAll(u => u.Id == userId) > 0;

            bool removedFromKeywords = false;
            if (keyword.Users.Count == 0)
                removedFromKeywords = _dbContext.Keywords.Remove(keyword) != null;

            bool isChangesSaved = _dbContext.SaveChanges() > 0;

            return (removedForUser && isChangesSaved, removedFromKeywords && isChangesSaved);
        }

        #region IDisposable

        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        #endregion
    }
}
