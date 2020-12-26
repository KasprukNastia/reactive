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
        private readonly IUserRepository _userRepository;
        private readonly IKeywordProvider _keywordProvider;
        private readonly IUserAuthHandler _userAuthHandler;

        public UserKeywordsMiddleware(
            RequestDelegate next,
            IUserAuthHandler userAuthHandler,
            IUserRepository userRepository,
            IKeywordProvider keywordProvider)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _userAuthHandler = userAuthHandler ?? throw new ArgumentNullException(nameof(userAuthHandler));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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
                        IObservable<KeywordOutput> userKeywords = _keywordProvider.GetListKeywords(
                            user.Id,
                            AsyncEnumerable.ToObservable(_userRepository.GetAllUserKeywords(user.Id))
                                .Select(k => new KeywordInput { Keyword = k.Word, Source = k.Source }));

                        return Observable.Create<KeywordRequest>(async observer =>
                        {
                            byte[] buffer;
                            WebSocketReceiveResult result;
                            KeywordRequest receivedRequest;
                            bool onlyConnected = true;
                            while (webSocket.State == WebSocketState.Open)
                            {
                                buffer = new byte[1024 * 4];
                                if (onlyConnected)
                                {
                                    receivedRequest = new KeywordRequest { OperationType = OperationType.Connected };
                                    onlyConnected = false;
                                }
                                else
                                {
                                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                                    receivedRequest = JsonConvert.DeserializeObject<KeywordRequest>(Encoding.UTF8.GetString(buffer, 0, result.Count));
                                }
                                observer.OnNext(receivedRequest);
                            }
                        })
                        .SelectMany(kr =>
                        {
                            if(kr.OperationType != OperationType.Connected)
                            {
                                userKeywords = kr.OperationType == OperationType.Subscribe ?
                                    userKeywords.Merge(_keywordProvider.GetOneKeyword(user.Id, kr)) :
                                    _keywordProvider.RemoveKeywordForUser(userKeywords, user.Id, kr);
                            }
                            return userKeywords;
                        })
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
