using Lesson12.Common.Src.Dto;
using Lesson12.Lesson12.Crypto_Service_Idl.Src.Service;
using Lesson12.Trade_Service.Src.Domain;
using Lesson12.Trade_Service.Src.Repository;
using Lesson12.Trade_Service_Idl.Src.Service;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Lesson12.Trade_Service.Src.Service.impl
{
    public class DefaultTradeService : ITradeService
    {
		private readonly ILogger _logger;
		private readonly IObservable<MessageDTO<MessageTrade>> _sharedStream;

		public DefaultTradeService(
			ICryptoService service, 
			ITradeRepository jdbcRepository,
			ITradeRepository mongoRepository)
		{
			
		}

		public IObservable<MessageDTO<MessageTrade>> TradesStream()
		{
			return _sharedStream;
		}

		private IObservable<MessageDTO<MessageTrade>> FilterAndMapTradingEvents(IObservable<Dictionary<string, object>> input)
		{
			// TODO: Add implementation to produce trading events
			throw new NotImplementedException();
		}

		private IObservable<Trade> MapToDomainTrade(IObservable<MessageDTO<MessageTrade>> input)
		{
			// TODO: Add implementation to mapping to com.example.part_10.domain.Trade
			throw new NotImplementedException();
		}

		private IObservable<int> ResilientlyStoreByBatchesToAllRepositories(
				IObservable<Trade> input,
				ITradeRepository tradeRepository1,
				ITradeRepository tradeRepository2)
		{
			throw new NotImplementedException();
		}
	}
}
