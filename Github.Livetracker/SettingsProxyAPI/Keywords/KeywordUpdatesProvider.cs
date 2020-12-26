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
    public class KeywordUpdatesProvider : IKeywordUpdatesProvider, IDisposable
    {
        private readonly IKeywordInfoRepository _keywordInfoRepository;

        private readonly string _dbConnStr;
        private readonly SqlTableDependency<KeywordInfo> _tableDependency;

        private readonly Subject<KeywordOutput> _allKeywordSequencesSubject;
        private readonly ConcurrentDictionary<string, IObservable<KeywordOutput>> _keywordsDict;

        private readonly Subject<int> _processedKeywordIds;

        public KeywordUpdatesProvider(
            IKeywordInfoRepository keywordInfoRepository, 
            string dbConnStr,
            int processedKeywordsBufferSize = 3)
        {
            _keywordInfoRepository = keywordInfoRepository ?? throw new ArgumentNullException(nameof(keywordInfoRepository));

            if (string.IsNullOrWhiteSpace(dbConnStr))
                throw new ArgumentNullException(nameof(dbConnStr));

            _dbConnStr = dbConnStr;
            _tableDependency = new SqlTableDependency<KeywordInfo>(_dbConnStr, nameof(UsersLivetrackerContext.KeywordInfos));
            _tableDependency.OnChanged += ListenChanges;
            _tableDependency.Start();

            _allKeywordSequencesSubject = new Subject<KeywordOutput>();
            _keywordsDict = new ConcurrentDictionary<string, IObservable<KeywordOutput>>();

            _processedKeywordIds = new Subject<int>();
            _processedKeywordIds.Buffer(processedKeywordsBufferSize)
                .Select(keywordInfoIds => keywordInfoRepository.SetKeywordsProcessed(keywordInfoIds.ToList()))
                .Subscribe();
        }

        public IObservable<KeywordOutput> GetKeywordSequence(KeywordInput keywordInput)
        {
            string identifier = $"{keywordInput.Keyword}&{keywordInput.Source}";
            if (_keywordsDict.TryGetValue(identifier, out IObservable<KeywordOutput> keywordObservable))
                return keywordObservable;

            keywordObservable = Observable.Merge(
                _allKeywordSequencesSubject.Where(k => $"{k.Keyword}&{k.Source}".Equals(identifier)),
                AsyncEnumerable.ToObservable(_keywordInfoRepository.GetAllUnprocessedKeywords(keywordInput.Keyword, keywordInput.Source))
                    .Select(keywordInfo => 
                    { 
                        return new KeywordOutput
                        {
                            Keyword = keywordInfo.Word,
                            Source = keywordInfo.Source,
                            FileName = keywordInfo.FileName,
                            RelativePath = keywordInfo.RelativePath,
                            FileUrl = keywordInfo.FileUrl,
                            RepositoryUrl = keywordInfo.RepositoryUrl
                        };
                    }));

            _keywordsDict.TryAdd(identifier, keywordObservable);

            return keywordObservable;
        }

        public bool RemoveKeywordSequence(KeywordInput keywordInput) =>
            _keywordsDict.TryRemove($"{keywordInput.Keyword}&{keywordInput.Source}", out IObservable<KeywordOutput> keywordObservable);

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

            _allKeywordSequencesSubject.OnNext(keywordInfo);
            _processedKeywordIds.OnNext(e.Entity.Id);
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
