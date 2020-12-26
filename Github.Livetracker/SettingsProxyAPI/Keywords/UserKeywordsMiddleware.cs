using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SettingsProxyAPI.Auth;
using SettingsProxyAPI.Models;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.Keywords
{
    public class UserKeywordsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserKeywordsRepository _userKeywordsRepository;
        private readonly IKeywordUpdatesProvider _keywordProvider;
        private readonly IUserAuthHandler _userAuthHandler;

        public UserKeywordsMiddleware(
            RequestDelegate next,
            IUserAuthHandler userAuthHandler,
            IUserKeywordsRepository userKeywordsRepository,
            IKeywordUpdatesProvider keywordProvider)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _userAuthHandler = userAuthHandler ?? throw new ArgumentNullException(nameof(userAuthHandler));
            _userKeywordsRepository = userKeywordsRepository ?? throw new ArgumentNullException(nameof(userKeywordsRepository));
            _keywordProvider = keywordProvider ?? throw new ArgumentNullException(nameof(keywordProvider));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/keywords" && context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                await _userAuthHandler.IdentifyUser(context)
                    .SelectMany(user =>
                    {
                        return Observable.Create<KeywordRequest>(async observer =>
                        {
                            byte[] buffer;
                            WebSocketReceiveResult result;
                            KeywordRequest receivedRequest;
                            observer.OnNext(new KeywordRequest { OperationType = OperationType.Connected });
                            while (webSocket.State == WebSocketState.Open)
                            {
                                buffer = new byte[1024 * 4];
                                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                                receivedRequest = JsonConvert.DeserializeObject<KeywordRequest>(Encoding.UTF8.GetString(buffer, 0, result.Count));
                                observer.OnNext(receivedRequest);
                            }
                        })
                        .SelectMany(keywordRequest =>
                        {
                            if (keywordRequest.OperationType == OperationType.Connected)
                            {
                                return _userKeywordsRepository.GetAllUserKeywords(user.Id)
                                    .ToObservable()
                                    .Select(k => new KeywordInput { Keyword = k.Word, Source = k.Source })
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
                        .Select(keywordInfo => JsonConvert.SerializeObject(keywordInfo))
                        .Do(onNext: async message =>
                        {
                            byte[] output = Encoding.UTF8.GetBytes(message);
                            await webSocket.SendAsync(new ArraySegment<byte>(output, 0, output.Length),
                                WebSocketMessageType.Text, true, CancellationToken.None);
                        },
                        onError: async exception =>
                        {
                            byte[] output = Encoding.UTF8.GetBytes(exception.Message);
                            await webSocket.SendAsync(new ArraySegment<byte>(output, 0, output.Length),
                                WebSocketMessageType.Text, true, CancellationToken.None);
                        });

                    })
                    .LastAsync();
            }
            
            if (_next != null)
                await _next.Invoke(context);
        }
    }
}
