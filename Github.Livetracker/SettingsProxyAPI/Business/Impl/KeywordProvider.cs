using Newtonsoft.Json;
using SettingsProxyAPI.Business.Interfaces;
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

namespace SettingsProxyAPI.Business.Impl
{
    public class KeywordProvider : IKeywordProvider
    {
        private readonly string _remoteKeywordServiceUri;
        private readonly int _keywordsBufferSize;

        private readonly Dictionary<string, IObservable<KeywordInfo>> _keywordsDict;

        public KeywordProvider(
            string remoteKeywordServiceUri, 
            int keywordsBufferSize = 20)
        {
            if (!Uri.IsWellFormedUriString(remoteKeywordServiceUri, UriKind.Absolute))
                throw new UriFormatException($"Bad formed URI {remoteKeywordServiceUri}");

            _remoteKeywordServiceUri = remoteKeywordServiceUri;
            _keywordsBufferSize = keywordsBufferSize;
            _keywordsDict = new Dictionary<string, IObservable<KeywordInfo>>();
        }

        public IObservable<KeywordInfo> GetListKeywords(IObservable<string> keywords) =>
            keywords.Select(k => GetOneKeyword(k)).Merge();

        public IObservable<KeywordInfo> GetOneKeyword(string keyword)
        {
            if (_keywordsDict.TryGetValue(keyword, out IObservable<KeywordInfo> keywordSubject))
                return keywordSubject;

            keywordSubject = GetKeywordSubjectFromRemote(keyword).ToObservable().Merge();
            
            _keywordsDict.Add(keyword, keywordSubject);

            return keywordSubject;
        }

        private async Task<IObservable<KeywordInfo>> GetKeywordSubjectFromRemote(string keyword)
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
