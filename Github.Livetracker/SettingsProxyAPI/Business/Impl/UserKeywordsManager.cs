using SettingsProxyAPI.AppCode.Auth;
using SettingsProxyAPI.Business.Interfaces;
using SettingsProxyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
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

        public async Task<IObservable<KeywordInfo>> OnUserConnected(int userId)
        {
            List<string> keywords = (await _userRepository.GetAllUserKeywords(userId))
                .Select(k => k.Word).ToList();

            return await _keywordProvider.GetListKeywords(keywords);
        }

        public async Task<IObservable<KeywordInfo>> AppendKeyword(IObservable<KeywordInfo> keywordsObservable, string keyword) =>
            Observable.Merge(keywordsObservable, await _keywordProvider.GetOneKeyword(keyword));
    }
}
