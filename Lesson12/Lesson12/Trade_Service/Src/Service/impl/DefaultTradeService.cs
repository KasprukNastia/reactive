using Lesson12.Common.Src.Dto;
using Lesson12.Common.Src.Service.Utils;
using Lesson12.Lesson12.Crypto_Service_Idl.Src.Service;
using Lesson12.Trade_Service.Src.Domain;
using Lesson12.Trade_Service.Src.Domain.Utils;
using Lesson12.Trade_Service.Src.Repository;
using Lesson12.Trade_Service_Idl.Src.Service;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Lesson12.Trade_Service.Src.Service.impl
{
    public class DefaultTradeService : ITradeService
    {
		private readonly ILogger<DefaultTradeService> _logger;
		private readonly IObservable<MessageDTO<MessageTrade>> _sharedStream;

		public DefaultTradeService(
			ILogger<DefaultTradeService> logger,
			ICryptoService service, 
			IEnumerable<ITradeRepository> tradeRepositories)
		{
			_logger = logger;
		}

		public IObservable<MessageDTO<MessageTrade>> TradesStream() => _sharedStream;

		private IObservable<MessageDTO<MessageTrade>> FilterAndMapTradingEvents(IObservable<Dictionary<string, object>> input)
		{
			// TODO: Add implementation to produce trading events
			return input.Where(m => MessageMapper.IsTradeMessageType(m)).Select(m => MessageMapper.MapToTradeMessage(m));
		}

		private IObservable<Trade> MapToDomainTrade(IObservable<MessageDTO<MessageTrade>> input)
		{
			// TODO: Add implementation to mapping to com.example.part_10.domain.Trade
			return input.Select(m => DomainMapper.MapToDomain(m));
		}

		private IObservable<int> ResilientlyStoreByBatchesToAllRepositories(
				IObservable<Trade> input,
				ITradeRepository tradeRepository1,
				ITradeRepository tradeRepository2)
		{
			return input.Buffer(TimeSpan.FromSeconds(1)).SelectMany(trades =>
			{
				List<Trade> tradesList = trades.ToList();

				return tradeRepository1.SaveAll(tradesList).Merge(tradeRepository2.SaveAll(tradesList));
			}).Timeout(TimeSpan.FromSeconds(1)).Retry(100);
		}
	}
}
