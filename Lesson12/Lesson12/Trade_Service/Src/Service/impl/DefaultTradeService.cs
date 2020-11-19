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
using System.Reactive.Subjects;

namespace Lesson12.Trade_Service.Src.Service.impl
{
    public class DefaultTradeService : ITradeService
    {
		private readonly ILogger<DefaultTradeService> _logger;
		private readonly ICryptoService _cryptoService;
		private readonly IEnumerable<ITradeRepository> _tradeRepositories;

		public DefaultTradeService(
			ILogger<DefaultTradeService> logger,
			ICryptoService service, 
			IEnumerable<ITradeRepository> tradeRepositories)
		{
			_logger = logger;
			_cryptoService = service ?? throw new ArgumentNullException(nameof(service));
			_tradeRepositories = tradeRepositories ?? throw new ArgumentNullException(nameof(tradeRepositories));
		}

		public IObservable<MessageDTO<MessageTrade>> TradesStream()
        {
			return _cryptoService.EventsStream()
				.Let(FilterAndMapTradingEvents)
				.Let(trades =>
				{
					trades.Let(MapToDomainTrade)
						.Let(f => ResilientlyStoreByBatchesToAllRepositories(f, _tradeRepositories.First(), _tradeRepositories.Last()))
						.Subscribe(new Subject<int>());

					return trades;
				});
		}


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
			var helperSubject = new BehaviorSubject<object>(new object());
			helperSubject.OnNext(new object());

			IObservable<(long, object)> bufferBoundaries = Observable.Interval(TimeSpan.FromSeconds(1)).Zip(helperSubject, (i, o) => (i, o));

			return input.Buffer(bufferBoundaries)
				.SelectMany(trades =>
				{
					List<Trade> tradesList = trades.ToList();

					return SafetySave(tradeRepository1, tradesList)
						.Merge(SafetySave(tradeRepository2, tradesList))
						.Do(onNext: i => { }, onCompleted: () => helperSubject.OnNext(new object()));
				});

			IObservable<int> SafetySave(ITradeRepository tradeRepository, List<Trade> tradesList) =>
				tradeRepository.SaveAll(tradesList).Timeout(TimeSpan.FromSeconds(1)).Retry(100);
		}
	}
}
