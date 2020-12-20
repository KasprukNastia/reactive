using Newtonsoft.Json;
using SettingsProxyAPI.Business.Interfaces;
using SettingsProxyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SettingsProxyAPI.Business.Impl
{
    public class KeywordProvider : IKeywordProvider
    {
        private readonly string _remoteKeywordServiceUri;
        private readonly int _keywordsBufferSize;

        private readonly Dictionary<string, ReplaySubject<KeywordInfo>> _keywordsDict;

        public KeywordProvider(
            string remoteKeywordServiceUri, 
            int keywordsBufferSize = 20)
        {
            if (!Uri.IsWellFormedUriString(remoteKeywordServiceUri, UriKind.Absolute))
                throw new UriFormatException($"Bad formed URI {remoteKeywordServiceUri}");

            _remoteKeywordServiceUri = remoteKeywordServiceUri;
            _keywordsBufferSize = keywordsBufferSize;
            _keywordsDict = new Dictionary<string, ReplaySubject<KeywordInfo>>();
        }

        public async Task<IObservable<KeywordInfo>> GetListKeywords(List<string> keywords)
        {
            if (keywords.Count == 0)
                return Observable.Empty<KeywordInfo>();

            List<IObservable<KeywordInfo>> keywordsObservables = 
                new List<IObservable<KeywordInfo>>(keywords.Count);

            foreach(string keyword in keywords)
                keywordsObservables.Add(await GetOneKeyword(keyword));

            return Observable.Merge(keywordsObservables);
        }

        public async Task<IObservable<KeywordInfo>> GetOneKeyword(string keyword)
        {
            if (_keywordsDict.TryGetValue(keyword, out ReplaySubject<KeywordInfo> keywordSubject))
                return keywordSubject;

            keywordSubject = await GetKeywordSubjectFromRemote(keyword);
            
            _keywordsDict.Add(keyword, keywordSubject);

            return keywordSubject;
        }

        private async Task<ReplaySubject<KeywordInfo>> GetKeywordSubjectFromRemote(string keyword)
        {
            using ClientWebSocket ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri($"{_remoteKeywordServiceUri}?keyword={keyword}"), CancellationToken.None);

            IObservable<KeywordInfo> keywordInfoObservable = Observable.Create<KeywordInfo>(async observer =>
            {
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string receivedJson;
                while (!result.EndOfMessage)
                {
                    buffer = new byte[1024 * 4];
                    result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    receivedJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    observer.OnNext(JsonConvert.DeserializeObject<KeywordInfo>(receivedJson));
                }
            });

            var replaySubject = new ReplaySubject<KeywordInfo>(bufferSize: _keywordsBufferSize);
            keywordInfoObservable.Subscribe(replaySubject);

            return replaySubject;
        }
    }
}
