using System;
using System.Reactive.Linq;
using Lesson12.Common.Src.Dto;
using Lesson12.Price_Service_Idl.Src.Service;
using Lesson12.Trade_Service_Idl.Src.Service;
using Microsoft.AspNetCore.Mvc;


namespace Lesson12.Controllers
{
    public class WSHandlerController : Controller
    {
		private readonly IPriceService _priceService;
		private readonly ITradeService _tradeService;

		public WSHandlerController(IPriceService priceService, ITradeService tradeService)
		{
			_priceService = priceService;
			_tradeService = tradeService;
		}

		public IObservable<MessageDTO<dynamic>> Handle(IObservable<long> input)
		{
			//return Observable.Merge<MessageDTO<dynamic>>(
			//		_priceService.PricesStream(input),
			//		_tradeService.TradesStream());

			throw new NotImplementedException();
		}
	}
}
