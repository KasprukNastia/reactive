using Newtonsoft.Json;
using SettingsProxyAPI.Models;
using System;
using System.Linq;
using System.Reactive.Linq;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.Keywords
{
    public class KeywordSubscriptionHandler : IKeywordSubscriptionHandler
    {
        private readonly IUserKeywordsRepository _userKeywordsRepository;
        private readonly IKeywordUpdatesProvider _keywordProvider;

        public KeywordSubscriptionHandler(
            IUserKeywordsRepository userKeywordsRepository,
            IKeywordUpdatesProvider keywordProvider)
        {
            _userKeywordsRepository = userKeywordsRepository ?? throw new ArgumentNullException(nameof(userKeywordsRepository));
            _keywordProvider = keywordProvider ?? throw new ArgumentNullException(nameof(keywordProvider));
        }

        public IObservable<string> Handle(User user, IObservable<KeywordSubscriptionRequest> userKeywordRequests)
        {
            return userKeywordRequests
                .SelectMany(keywordRequest =>
                {
                    if (keywordRequest.OperationType == OperationType.Connected)
                    {
                        return _userKeywordsRepository.GetAllUserKeywords(user.Id)
                            .ToObservable()
                            .Select(k => new KeywordRequest { Keyword = k.Word, Source = k.Source })
                            .Select(k => _keywordProvider.GetKeywordSequence(k))
                            .Merge();
                    }
                    else if (keywordRequest.OperationType == OperationType.Subscribe)
                    {
                        _userKeywordsRepository.AddKeywordForUser(user.Id, keywordRequest.Keyword, keywordRequest.Source);
                        return _keywordProvider.GetKeywordSequence(keywordRequest);
                    }
                    else
                    {
                        (bool removedForUser, bool removedFromKeywords) =
                            _userKeywordsRepository.RemoveKeywordForUser(user.Id, keywordRequest.Keyword, keywordRequest.Source);
                        if (removedFromKeywords)
                            _keywordProvider.RemoveKeywordSequence(keywordRequest);
                        return Observable.Empty<KeywordOutput>();
                    }
                })
                .Where(keywordOutput => _userKeywordsRepository.GetAllUserKeywords(user.Id)
                    .Any(k => k.Word.Equals(keywordOutput.Keyword) && k.Source.Equals(keywordOutput.Source)))
                .Select(keywordInfo => JsonConvert.SerializeObject(keywordInfo));
        }
    }
}
