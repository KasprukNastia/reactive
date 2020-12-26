using SettingsProxyAPI.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using UsersLivetrackerConfigDAL;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.Keywords
{
    public class KeywordProvider : IKeywordProvider, IDisposable
    {
        private readonly string _dbConnStr;
        private readonly SqlTableDependency<KeywordInfo> _tableDependency;

        private readonly IKeywordRepository _keywordRepository;

        private readonly Subject<KeywordOutput> _allKeywordOutputsSubject;
        private readonly ConcurrentDictionary<string, IObservable<KeywordOutput>> _keywordsDict;

        public KeywordProvider(string dbConnStr,
            IKeywordRepository keywordRepository)
        {
            if (string.IsNullOrWhiteSpace(dbConnStr))
                throw new ArgumentNullException(nameof(dbConnStr));

            _dbConnStr = dbConnStr;
            _keywordRepository = keywordRepository ?? throw new ArgumentNullException(nameof(keywordRepository));
            _tableDependency = new SqlTableDependency<KeywordInfo>(_dbConnStr, nameof(UsersLivetrackerContext.KeywordInfos));
            _tableDependency.OnChanged += ListenChanges;
            _tableDependency.Start();

            _allKeywordOutputsSubject = new Subject<KeywordOutput>();
            _keywordsDict = new ConcurrentDictionary<string, IObservable<KeywordOutput>>();
        }

        public IObservable<KeywordOutput> GetListKeywords(int userId, IObservable<KeywordInput> keywords) =>
            keywords.Select(k => GetOneKeyword(userId, k)).Merge();

        public IObservable<KeywordOutput> GetOneKeyword(int userId, KeywordInput keywordInput)
        {
            string identifier = $"{keywordInput.Keyword}&{keywordInput.Source}";
            if (_keywordsDict.TryGetValue(identifier, out IObservable<KeywordOutput> keywordObservable))
                return keywordObservable;

            _keywordRepository.AddKeywordForUser(userId, keywordInput.Keyword, keywordInput.Source);

            keywordObservable = 
                _allKeywordOutputsSubject.Where(k => $"{k.Keyword}&{k.Source}".Equals(identifier));

            _keywordsDict.TryAdd(identifier, keywordObservable);

            return keywordObservable;
        }

        public IObservable<KeywordOutput> RemoveKeywordForUser(
            IObservable<KeywordOutput> userKeywordsObservable, 
            int userId, 
            KeywordInput keywordInput)
        {
            (bool removedForUser, bool removedFromKeywords) = 
                _keywordRepository.RemoveKeywordForUser(userId, keywordInput.Keyword, keywordInput.Source);

            if (removedFromKeywords)
                _keywordsDict.TryRemove(
                    $"{keywordInput.Keyword}&{keywordInput.Source}", 
                    out IObservable<KeywordOutput> keywordObservable);

            return userKeywordsObservable.Where(k => 
            {
                var res = !(k.Keyword.Equals(keywordInput.Keyword) && k.Source.Equals(keywordInput.Source));
                return res;
            });
        }

        private void ListenChanges(object sender, RecordChangedEventArgs<KeywordInfo> e)
        {
            if (e.ChangeType != ChangeType.Insert)
                return;

            var keywordInfo = new KeywordOutput
            {
                Keyword = e.Entity.Word,
                Source = e.Entity.Source,
                FileName = e.Entity.FileName,
                RelativePath = e.Entity.RelativePath,
                FileUrl = e.Entity.FileUrl,
                RepositoryUrl = e.Entity.RepositoryUrl
            };

            _allKeywordOutputsSubject.OnNext(keywordInfo);
        }

        #region IDisposable

        public void Dispose()
        {
            _tableDependency.Stop();
            _tableDependency.Dispose();
        }

        #endregion
    }
}
