﻿using SettingsProxyAPI.Models;
using System;
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
    public class LiveKeywordUpdatesProcessor : ILiveKeywordUpdatesProcessor, IDisposable
    {
        private readonly string _dbConnStr;
        private readonly SqlTableDependency<KeywordInfo> _tableDependency;

        private readonly Subject<KeywordOutput> _allKeywordSequencesSubject;
        private readonly Subject<int> _processedKeywordIds;

        public IObservable<KeywordOutput> AllKeywordSequencesSubject => _allKeywordSequencesSubject;

        public LiveKeywordUpdatesProcessor(
            IKeywordInfoRepository keywordInfoRepository,
            string dbConnStr,
            int collectInBufferSeconds = 5,
            int processedKeywordsSaveTimeoutSeconds = 5,
            int processedKeywordsSaveRetryCount = 20)
        {
            if (string.IsNullOrWhiteSpace(dbConnStr))
                throw new ArgumentNullException(nameof(dbConnStr));

            _dbConnStr = dbConnStr;
            _tableDependency = new SqlTableDependency<KeywordInfo>(_dbConnStr, nameof(GithubLivetrackerContext.KeywordInfos));
            _tableDependency.OnChanged += ListenChanges;
            _tableDependency.Start();

            _allKeywordSequencesSubject = new Subject<KeywordOutput>();

            var helperSubject = new BehaviorSubject<object>(new object());
            helperSubject.OnNext(new object());
            IObservable<(long, object)> bufferBoundaries = 
                Observable.Interval(TimeSpan.FromSeconds(collectInBufferSeconds))
                    .Zip(helperSubject, (i, o) => (i, o));

            _processedKeywordIds = new Subject<int>();
            _processedKeywordIds.Buffer(bufferBoundaries)
                .Select(keywordInfoIds => keywordInfoRepository.SetRecordsProcessed(keywordInfoIds.ToList()))
                .Timeout(TimeSpan.FromSeconds(processedKeywordsSaveTimeoutSeconds))
                .Retry(processedKeywordsSaveRetryCount)
                .Do(
                    onNext: i => { }, 
                    onError: e => helperSubject.OnNext(new object()),
                    onCompleted: () => helperSubject.OnNext(new object()))
                .Subscribe();
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
