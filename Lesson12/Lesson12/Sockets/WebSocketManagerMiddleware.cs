using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Lesson12.Sockets
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandler _webSocketHandler { get; set; }

        public WebSocketManagerMiddleware(RequestDelegate next,
                                            WebSocketHandler webSocketHandler)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.Out.WriteLine("There");
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await _webSocketHandler.OnConnected(socket);

            var buffer = new byte[1024 * 4];

            socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None)
                    .ToObservable()
                    .SelectMany(r => _webSocketHandler.ReceiveAsync(socket, r, buffer))
                    .Subscribe(e => Debug.WriteLine($"Received"));

            //await Receive(socket, async (result, buffer) =>
            //{
            //    if (result.MessageType == WebSocketMessageType.Text)
            //    {
            //        await _webSocketHandler.ReceiveAsync(socket, result, buffer);
            //        return;
            //    }

            //    else if (result.MessageType == WebSocketMessageType.Close)
            //    {
            //        await _webSocketHandler.OnDisconnected(socket);
            //        return;
            //    }

            //});
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            //while (socket.State == WebSocketState.Open)
            //{
                

            //    //handleMessage(result, buffer);
            //}
        }
    }
}
