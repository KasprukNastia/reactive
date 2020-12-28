using SettingsProxyAPI.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.Keywords
{
    public class KeywordUpdatesProvider : IKeywordUpdatesProvider
    {
        private readonly IKeywordInfoRepository _keywordInfoRepository;
        private readonly ILiveKeywordUpdatesProcessor _keywordUpdatesProvider;

        private readonly ConcurrentDictionary<string, IObservable<KeywordOutput>> _keywordsDict;

        public int ObservedKeywordsCount => _keywordsDict.Count;

        public KeywordUpdatesProvider(
            IKeywordInfoRepository keywordInfoRepository,
            ILiveKeywordUpdatesProcessor keywordUpdatesProvider)
        {
            _keywordInfoRepository = keywordInfoRepository ?? throw new ArgumentNullException(nameof(keywordInfoRepository));
            _keywordUpdatesProvider = keywordUpdatesProvider ?? throw new ArgumentNullException(nameof(keywordUpdatesProvider));

            _keywordsDict = new ConcurrentDictionary<string, IObservable<KeywordOutput>>();
        }

        public IObservable<KeywordOutput> GetKeywordSequence(KeywordRequest keywordRequest)
        {
            string identifier = $"{keywordRequest.Keyword}&{keywordRequest.Source}";
            if (_keywordsDict.TryGetValue(identifier, out IObservable<KeywordOutput> keywordObservable))
                return keywordObservable;

            keywordObservable = Observable.Merge(
                _keywordUpdatesProvider.AllKeywordSequencesSubject.Where(k => $"{k.Keyword}&{k.Source}".Equals(identifier)),
                AsyncEnumerable.ToObservable(_keywordInfoRepository.GetAllUnprocessedRecords(keywordRequest.Keyword, keywordRequest.Source))
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

        public bool RemoveKeywordSequence(KeywordRequest keywordInput) =>
            _keywordsDict.TryRemove($"{keywordInput.Keyword}&{keywordInput.Source}", out IObservable<KeywordOutput> keywordObservable);
    }
}
