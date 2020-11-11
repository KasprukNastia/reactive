using Dapper;
using Lesson12.Trade_Service.Src.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Lesson12.Trade_Service.Src.Repository.impl
{
    public class H2TradeRepository : ITradeRepository
    {
        private readonly ILogger<H2TradeRepository> _log;
        private readonly string _connectionString;

        private static string INIT_DB =
            "CREATE TABLE trades (" +
                "id varchar(48), " +
                "trade_timestamp bigint, " +
                "price float, " +
                "amount float, " +
                "currency varchar(8)," +
                "market varchar(64))";

        private static readonly string TRADES_COUNT_QUERY = "SELECT COUNT(*) as cnt FROM trades";

        private static readonly string INSERT_TRADE_QUERY =
            "INSERT INTO trades (id, trade_timestamp, price, amount, currency, market) " +
            "VALUES (@1, @2, @3, @4, @5, @6)";

        public H2TradeRepository(ILogger<H2TradeRepository> log, string connectionString)
        {
            _log = log;
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            _connectionString = connectionString;

            InitDB();
            PingDB();
            ReportDbStatistics();
        }

        private void InitDB()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                using SqlCommand command = new SqlCommand(INIT_DB, con);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
            }
        }

        private void PingDB()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                int result = con.QueryFirstOrDefault<int>("SELECT 6");
                _log.LogInformation("RESULT FOR SELECT 6 QUERY: " + result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
            }
        }

        // Stats: log the amount of stored trades to log every 5 seconds
        private void ReportDbStatistics()
        {
            Observable.Interval(TimeSpan.FromSeconds(5))
                .SelectMany(i => GetTradeStats())
                .Do(count => _log.LogInformation("------------- [DB STATS] ------------ Trades stored in DB: " + count))
                .SubscribeOn(NewThreadScheduler.Default)
                .Subscribe();
        }

        public IObservable<int> SaveAll(List<Trade> input)
        {
            return StoreTradesInDb(input)
                .Do(e => _log.LogInformation("--- [DB] --- Inserted " + e + " trades into DB"));
        }

        private IObservable<long> GetTradeStats()
        {
            // TODO: Return the current amount of stored trades
            return Observable.Defer(() =>
            // TODO: Instead of IObservable.empty(), do a query to H2 database using h2Client.withHandle(...)
            // TODO: Use Handle.createQuery & TRADES_COUNT_QUERY with SQL
            // TODO: Map result row by row to get the result of query
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                con.Open();
                try
                {
                    long result = con.QuerySingle<long>(TRADES_COUNT_QUERY);
                    return Observable.Return(result);
                }
                catch (Exception ex)
                {
                    _log.LogError(ex.Message);
                    throw ex;
                }
            });
        }

        private IObservable<int> StoreTradesInDb(List<Trade> trades)
        {
            // TODO: Instead of IObservable.never()
            // TODO: Use h2Client to create handle, build UPDATE statement, use transactional support!
            // TODO: Add all trades to update using buildInsertStatement(...) method
            // TODO: Return the amount of stored rows

            int affectedRows = 0;
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlTransaction trans = con.BeginTransaction();

                foreach (var param in BuildInsertStatement(trades))
                    affectedRows += con.Execute(INSERT_TRADE_QUERY, param.Value, trans);

            return Observable.Return(affectedRows);
        }

        // --- Helper methods --------------------------------------------------

        // TODO: Use this method in storeTradesInDb(...) method
        private Dictionary<string, DynamicParameters> BuildInsertStatement(List<Trade> trades)
        {
            return trades.ToDictionary(t => t.Id, t =>
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@1", t.Id },
                    { "@2", t.Timestamp },
                    { "@3", t.Price },
                    { "@4", t.Amount },
                    { "@5", t.Currency },
                    { "@6", t.Market }
                };
                return new DynamicParameters(dictionary);
            });
        }
    }
}
