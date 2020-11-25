using System;
using System.Reactive.Linq;
using Lesson12.Price_Service_Idl.Src.Service;
using Lesson12.Trade_Service_Idl.Src.Service;
using Microsoft.AspNetCore.Mvc;

namespace Lesson12.Sockets
{
    public class WSHandler
	{
		private readonly IPriceService _priceService;
		private readonly ITradeService _tradeService;

		public WSHandler(
			IPriceService priceService, 
			ITradeService tradeService)
		{
			_priceService = priceService;
			_tradeService = tradeService;
		}

		[HttpGet]
		public IObservable<dynamic> Handle(IObservable<string> inbound)
		{
            return Observable.Merge<dynamic>(
				_priceService.PricesStream(inbound.Let(HandleRequestedAveragePriceIntervalValue)),
                _tradeService.TradesStream());
		}

		private static IObservable<long> HandleRequestedAveragePriceIntervalValue(IObservable<string> requestedInterval)
		{
			// TODO: input may be incorrect, pass only correct interval
			// TODO: ignore invalid values (empty, non number, <= 0, > 60)
			return requestedInterval
				.Where(i => !string.IsNullOrWhiteSpace(i) && long.TryParse(i, out long interval) && interval > 0 && interval <= 60)
				.Select(i => long.Parse(i));
		}
	}
}
