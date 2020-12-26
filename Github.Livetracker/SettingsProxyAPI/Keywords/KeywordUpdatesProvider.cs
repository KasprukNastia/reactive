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

namespace SettingsProxyAPI.Keywords
{
    public class KeywordUpdatesProvider : IKeywordUpdatesProvider, IDisposable
    {
        private readonly string _dbConnStr;
        private readonly SqlTableDependency<KeywordInfo> _tableDependency;

        private readonly Subject<KeywordOutput> _allKeywordOutputsSubject;
        private readonly ConcurrentDictionary<string, IObservable<KeywordOutput>> _keywordsDict;

        public KeywordUpdatesProvider(string dbConnStr)
        {
            if (string.IsNullOrWhiteSpace(dbConnStr))
                throw new ArgumentNullException(nameof(dbConnStr));

            _dbConnStr = dbConnStr;
            _tableDependency = new SqlTableDependency<KeywordInfo>(_dbConnStr, nameof(UsersLivetrackerContext.KeywordInfos));
            _tableDependency.OnChanged += ListenChanges;
            _tableDependency.Start();

            _allKeywordOutputsSubject = new Subject<KeywordOutput>();
            _keywordsDict = new ConcurrentDictionary<string, IObservable<KeywordOutput>>();
        }

        public IObservable<KeywordOutput> GetKeywordSequence(KeywordInput keywordInput)
        {
            string identifier = $"{keywordInput.Keyword}&{keywordInput.Source}";
            if (_keywordsDict.TryGetValue(identifier, out IObservable<KeywordOutput> keywordObservable))
                return keywordObservable;

            keywordObservable =
                _allKeywordOutputsSubject.Where(k => $"{k.Keyword}&{k.Source}".Equals(identifier));

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
