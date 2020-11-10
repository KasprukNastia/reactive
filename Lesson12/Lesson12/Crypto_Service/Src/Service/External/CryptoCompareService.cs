using Lesson12.Crypto_Service.Src.Service.External.Utils;
using Lesson12.Lesson12.Crypto_Service_Idl.Src.Service;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Lesson12.Crypto_Service.Src.Service.External
{
    public class CryptoCompareService : ICryptoService
    {
        public static readonly int CACHE_SIZE = 3;

        private readonly IObservable<Dictionary<string, object>> _reactiveCryptoListener;

        public CryptoCompareService(ILogger logger)
        {
            _reactiveCryptoListener = new CryptoCompareClient(logger)
                    .Connect(
                        new List<string> { "5~CCCAGG~BTC~USD", "0~Coinbase~BTC~USD", "0~Cexio~BTC~USD" }.ToObservable(),
                        new List<IMessageUnpacker> { new PriceMessageUnpacker(), new TradeMessageUnpacker() }
                    )
                    .Let(ProvideResilience)
                    .Let(ProvideCaching);
        }

        public IObservable<Dictionary<string, object>> EventsStream()
        {
            return _reactiveCryptoListener;
        }

        // TODO: implement resilience such as retry with delay
        public static IObservable<T> ProvideResilience<T>(IObservable<T> input)
        {
            throw new NotImplementedException();
        }

        // TODO: implement caching of 3 last elements & multi subscribers support
        public static IObservable<T> ProvideCaching<T>(IObservable<T> input)
        {
            throw new NotImplementedException();
        }
    }
}
