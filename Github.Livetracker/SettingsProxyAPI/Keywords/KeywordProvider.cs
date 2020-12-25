using Newtonsoft.Json;
using SettingsProxyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly Dictionary<string, IObservable<KeywordOutput>> _keywordsDict;

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
            _keywordsDict = new Dictionary<string, IObservable<KeywordOutput>>();
        }

        public IObservable<KeywordOutput> GetListKeywords(IObservable<string> keywords) =>
            keywords.Select(k => GetOneKeyword(k)).Merge();

        public IObservable<KeywordOutput> GetOneKeyword(string keyword)
        {
            if (_keywordsDict.TryGetValue(keyword, out IObservable<KeywordOutput> keywordSubject))
                return keywordSubject;

            keywordSubject = AddKeywordToObserveAsync(keyword).ToObservable().Merge();
            
            _keywordsDict.Add(keyword, keywordSubject);

            return keywordSubject;
        }

        private async Task<IObservable<KeywordOutput>> AddKeywordToObserveAsync(string keyword)
        {
            await _keywordRepository.TryAddKeywordAsync(new Keyword { Word = keyword });

            return _allKeywordOutputsSubject.Where(k => k.Keyword.Equals(keyword));
        }

        private void ListenChanges(object sender, RecordChangedEventArgs<KeywordInfo> e)
        {
            if (e.ChangeType != ChangeType.Insert)
                return;

            var keywordInfo = new KeywordOutput
            {
                Keyword = e.Entity.Word,
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
