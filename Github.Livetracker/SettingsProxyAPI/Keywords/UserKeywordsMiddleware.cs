using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SettingsProxyAPI.Auth.WebSockets;
using SettingsProxyAPI.Models;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SettingsProxyAPI.Keywords
{
    public class UserKeywordsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketsAuthHandler _userAuthHandler;
        private readonly IKeywordSubscriptionHandler _userKeywordSubscriptionHandler;

        public UserKeywordsMiddleware(
            RequestDelegate next,
            IWebSocketsAuthHandler userAuthHandler,
            IKeywordSubscriptionHandler userKeywordSubscriptionHandler)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _userAuthHandler = userAuthHandler ?? throw new ArgumentNullException(nameof(userAuthHandler));
            _userKeywordSubscriptionHandler = userKeywordSubscriptionHandler ?? throw new ArgumentNullException(nameof(userKeywordSubscriptionHandler));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/keywords" && context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                await _userAuthHandler.IdentifyUser(context.Request.Query["access_token"])
                    .SelectMany(user =>
                    {
                        return _userKeywordSubscriptionHandler
                            .Handle(
                                user: user,
                                userKeywordRequests: Observable.Create<KeywordSubscriptionRequest>(async observer =>
                                {
                                    byte[] buffer;
                                    WebSocketReceiveResult result;
                                    KeywordSubscriptionRequest receivedRequest;
                                    observer.OnNext(new KeywordSubscriptionRequest { OperationType = OperationType.Connected });
                                    while (webSocket.State == WebSocketState.Open)
                                    {
                                        buffer = new byte[1024 * 4];
                                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                                        receivedRequest = JsonConvert.DeserializeObject<KeywordSubscriptionRequest>(
                                            Encoding.UTF8.GetString(buffer, 0, result.Count));
                                        observer.OnNext(receivedRequest);
                                    }
                                }))
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
                                await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal server error", CancellationToken.None);
                            });
                    })
                    .LastAsync();
            }
            
            if (_next != null)
                await _next.Invoke(context);
        }
    }
}
