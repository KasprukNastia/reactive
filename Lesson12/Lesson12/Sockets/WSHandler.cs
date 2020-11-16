using System;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson12.Price_Service_Idl.Src.Service;
using Lesson12.Trade_Service_Idl.Src.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lesson12.Sockets
{
    public class WSHandler : WebSocketHandler
	{
		private readonly IPriceService _priceService;
		private readonly ITradeService _tradeService;

		public WSHandler(
			ConnectionManager webSocketConnectionManager, 
			IPriceService priceService, 
			ITradeService tradeService) 
			: base(webSocketConnectionManager)
		{
			_priceService = priceService;
			_tradeService = tradeService;
		}

		public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{
			return Task.FromResult(
				Observable.Return(Encoding.UTF8.GetString(buffer, 0, result.Count))
					.Let(Handle).Select(m => JsonConvert.SerializeObject(m))
					.Select(async m => await SendMessageToAllAsync(m as string)));
		}

		[HttpGet]
		private IObservable<dynamic> Handle(IObservable<string> input)
		{
            return Observable.Merge<dynamic>(
				_priceService.PricesStream(input.Let(HandleRequestedAveragePriceIntervalValue)),
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
