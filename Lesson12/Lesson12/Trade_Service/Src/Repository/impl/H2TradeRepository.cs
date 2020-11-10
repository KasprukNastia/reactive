using Lesson12.Trade_Service.Src.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson12.Trade_Service.Src.Repository.impl
{
    public class H2TradeRepository : ITradeRepository
    {
        private readonly ILogger _log;

        private static string INIT_DB =
            "CREATE TABLE trades (" +
                "id varchar(48), " +
                "trade_timestamp long, " +
                "price float, " +
                "amount float, " +
                "currency varchar(8)," +
                "market varchar(64))";

        private static readonly string TRADES_COUNT_QUERY = "SELECT COUNT(*) as cnt FROM trades";

        private static readonly string INSERT_TRADE_QUERY =
            "INSERT INTO trades (id, trade_timestamp, price, amount, currency, market) " +
            "VALUES ($1, $2, $3, $4, $5, $6)";

        public IObservable<int> SaveAll(List<Trade> input)
        {
            throw new NotImplementedException();
        }
    }
}
