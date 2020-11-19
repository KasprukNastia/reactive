using Lesson12.Trade_Service.Src.Domain;
using Lesson12.Trade_Service.Src.Domain.Utils;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Lesson12.Trade_Service.Src.Repository.impl
{
    public class MongoTradeRepository : ITradeRepository
    {
		private static readonly string DB_NAME = "crypto";
		private static readonly string COLLECTION_NAME = "trades";

		private readonly ILogger<MongoTradeRepository> _log;
		private readonly IMongoCollection<BsonDocument> _collection;

		public MongoTradeRepository(ILogger<MongoTradeRepository> log, MongoClient client)
		{
			_log = log;
			_collection = client.GetDatabase(DB_NAME)
				.GetCollection<BsonDocument>(COLLECTION_NAME);

			ReportDbStatistics();
		}


		private void ReportDbStatistics()
		{
			Observable.Interval(TimeSpan.FromSeconds(5))
				.SelectMany(i => GetTradeStats())
				.Do(count => _log.LogWarning("------------- [DB STATS] ------------ Trades " + "stored in DB: " + count))
				.SubscribeOn(NewThreadScheduler.Default)
				.Subscribe();
		}

		private IObservable<long> GetTradeStats()
		{
			// TODO: Return the current amount of stored trades
			return Observable.Return(_collection.EstimatedDocumentCount());
		}

		public IObservable<int> SaveAll(List<Trade> trades)
		{
			return StoreInMongo(trades.Select(DomainMapper.MapToMongoDocument).ToList());
		}

		private IObservable<int> StoreInMongo(List<BsonDocument> trades)
		{
			if(trades.Count > 0)
				_collection.InsertMany(trades);
			return Observable.Return(1);
		}
	}
}
