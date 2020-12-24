using SettingsProxyAPI.Business.Interfaces;
using SettingsProxyAPI.Models;
using System;
using System.Linq;
using System.Reactive.Linq;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.Business.Impl
{
    public class UserKeywordsManager : IUserKeywordsManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IKeywordProvider _keywordProvider;

        public UserKeywordsManager(
            IUserRepository userRepository, 
            IKeywordProvider keywordProvider)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _keywordProvider = keywordProvider ?? throw new ArgumentNullException(nameof(keywordProvider));
        }

        public IObservable<KeywordInfo> OnUserConnected(int userId) =>
            _keywordProvider.GetListKeywords(AsyncEnumerable.ToObservable(_userRepository.GetAllUserKeywords(userId)).Select(k => k.Word));
    }
}
