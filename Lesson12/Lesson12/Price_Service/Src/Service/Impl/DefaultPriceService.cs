using Lesson12.Common.Src.Dto;
using Lesson12.Common.Src.Service.Utils;
using Lesson12.Lesson12.Crypto_Service_Idl.Src.Service;
using Lesson12.Price_Service_Idl.Src.Service;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Lesson12.Price_Service.Src.Service.Impl
{
    public class DefaultPriceService : IPriceService
    {
		private static readonly long DEFAULT_AVG_PRICE_INTERVAL = 30L;

		private readonly ILogger<DefaultPriceService> _logger;
		private readonly IObservable<MessageDTO<float>> _sharedStream;

		public DefaultPriceService(ILogger<DefaultPriceService> logger, ICryptoService cryptoService)
		{
			_logger = logger;
			_sharedStream = cryptoService.EventsStream()
				.Let(SelectOnlyPriceUpdateEvents)
				.Let(CurrentPrice);
		}

		public IObservable<MessageDTO<float>> PricesStream(IObservable<long> intervalPreferencesStream)
		{
			return _sharedStream.Let(mainFlow => Observable.Merge(
				mainFlow,
				AveragePrice(intervalPreferencesStream, mainFlow)));
		}

		// FIXME:
		// 1) JUST FOR WARM UP: .map() incoming Dictionary<string, object> to MessageDTO. For that purpose use MessageDTO.price()
		//    NOTE: Incoming Dictionary<string, object> contains keys PRICE_KEY and CURRENCY_KEY
		//    NOTE: Use MessageMapper utility class for message validation and transformation
		// Visible for testing
		IObservable<Dictionary<string, object>> SelectOnlyPriceUpdateEvents(IObservable<Dictionary<string, object>> input)
		{
			// TODO: filter only Price messages
			// TODO: verify that price message are valid
			// HINT: Use MessageMapper methods to perform filtering and validation

			return input.Where(m => MessageMapper.IsPriceMessageType(m) && MessageMapper.IsValidPriceMessage(m));
		}

		// Visible for testing
		IObservable<MessageDTO<float>> CurrentPrice(IObservable<Dictionary<string, object>> input)
		{
			// TODO map to Statistic message using MessageMapper.mapToPriceMessage

			return input.Select(m => MessageMapper.MapToPriceMessage(m));
		}

		// 1.1)   TODO Collect crypto currency price during the interval of seconds
		//        HINT consider corner case when a client did not send any info about interval (add initial interval (mergeWith(...)))
		//        HINT use window + switchMap
		// 1.2)   TODO group collected MessageDTO results by currency
		//        HINT for reduce consider to reuse Sum.empty and Sum#add
		// 1.3.2) TODO calculate average for reduced Sum object using Sun#avg
		// 1.3.3) TODO map to Statistic message using MessageDTO#avg()

		// Visible for testing
		// TODO: Remove as should be implemented by trainees
		IObservable<MessageDTO<float>> AveragePrice(IObservable<long> requestedInterval,
				IObservable<MessageDTO<float>> priceData)
		{
			// 1.1
			return Observable.Concat(Observable.Return(DEFAULT_AVG_PRICE_INTERVAL), requestedInterval)
				.SelectMany(interval => priceData.Window(TimeSpan.FromMilliseconds(interval)))
				.Switch()
				// 1.2 + 1.3.2
				.GroupBy(m => m.Currency)
				.SelectMany(grouped => grouped.Aggregate(Sum.Empty(), (sum, mes) => sum.Add(mes.Data), sum => sum.Avg())
						.Select(avg => MessageDTO<float>.Avg(avg, grouped.Key, "Local"))
				);
		}
	}
}
