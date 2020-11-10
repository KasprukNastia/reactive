using Lesson12.Common.Src.Dto;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Lesson12.Trade_Service.Src.Domain.Utils
{
    public class DomainMapper
    {
		public static Trade MapToDomain(MessageDTO<MessageTrade> tradeMessageDTO)
		{
			Trade trade = new Trade();

			trade.Id = Guid.NewGuid().ToString();
			trade.Price = tradeMessageDTO.Data.Price;
			trade.Amount = tradeMessageDTO.Data.Amount;
			trade.Currency = tradeMessageDTO.Currency;
			trade.Market = tradeMessageDTO.Market;
			trade.Timestamp = tradeMessageDTO.Timestamp;

			return trade;
		}

		public static BsonDocument MapToMongoDocument(Trade trade)
		{
			var dictionary = new Dictionary<string, object>();
			foreach (var propertyInfo in trade.GetType().GetProperties())
					dictionary[propertyInfo.Name] = propertyInfo.GetValue(trade);

			return new BsonDocument(dictionary);
		}
	}
}
