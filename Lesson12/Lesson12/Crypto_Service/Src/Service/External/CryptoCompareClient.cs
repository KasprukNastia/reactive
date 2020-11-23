using H.Socket.IO;
using Lesson12.Crypto_Service.Src.Service.External.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Lesson12.Crypto_Service.Src.Service.External
{
    public class CryptoCompareClient
    {
        private readonly ILogger<CryptoCompareClient> _logger;

        public CryptoCompareClient(ILogger<CryptoCompareClient> logger)
        {
            _logger = logger;
        }

        public IObservable<Dictionary<string, object>> Connect(
            IObservable<string> input,
            ICollection<IMessageUnpacker> unpackers)
        {
            return Observable.Defer(() => Observable.Create<Dictionary<string, object>>(async (sink, token) =>
            {
                var taskCompletionSource = new TaskCompletionSource<dynamic>();
                SocketIoClient socket = new SocketIoClient();

                Uri connectionUri = new Uri("https://streamer.cryptocompare.com");
                // await socket.ConnectAsync(connectionUri);
                _logger.LogInformation("[EXTERNAL-SERVICE] Connecting to CryptoCompare.com ...");

                Func<Task> closeSocket = async () =>
                {
                    await socket.DisconnectAsync();
                    _logger.LogInformation("[EXTERNAL-SERVICE] Connection to CryptoCompare.com closed");
                };

                socket.Connected += (sender, args) =>
                {
                    input.Subscribe(v =>
                        {
                            string[] subscription = {v};
                            Dictionary<string, object> subs = new Dictionary<string, object>();
                            subs.Add("subs", subscription);
                            socket.Emit("SubAdd", subs);
                        },
                        onError: e => sink.OnError(e));
                };

                socket.On("m", async args =>
                {
                    string message = args;
                    string messageType = message.Substring(0, message.IndexOf("~"));
                    foreach (IMessageUnpacker unpacker in unpackers)
                    {
                        if (unpacker.Supports(messageType))
                        {
                            try
                            {
                                // Console.Out.WriteLine(message);
                                sink.OnNext(unpacker.Unpack(message));
                            }
                            catch (Exception e)
                            {
                                Console.Error.WriteLine(e);
                                sink.OnError(e);
                                await closeSocket.Invoke();
                            }

                            break;
                        }
                    }
                });

                socket.ErrorReceived += (sender, args) => sink.OnError(new Exception(args.Value));
                socket.Disconnected += (sender, args) => sink.OnCompleted();

                token.Register(async () => await closeSocket());
                await socket.ConnectAsync(connectionUri, token);

                await taskCompletionSource.Task;
            }));
        }
    }
}