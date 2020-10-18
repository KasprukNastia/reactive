using Reactive.Streams;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson11.Distributed_Transactions
{
    public class DatabasesIntegration
    {
		private readonly IDatabaseApi _oracleDb;
		private readonly IDatabaseApi _fileDb;

		public DatabasesIntegration(IDatabaseApi oracleDb, IDatabaseApi fileDb)
		{
			_oracleDb = oracleDb;
			_fileDb = fileDb;
		}

		public IObservable<int> StoreToDatabases(IObservable<int> integerFlux)
		{
			// TODO: Main) Write data to both databases
			// TODO: 1) Ensure Transaction is rolled back in case of failure
			// TODO: 2) Ensure All transactions are rolled back ion case any of write operations fails
			// TODO: 3) Ensure Transaction lasts less than 1 sec

			throw new NotImplementedException();
		}
	}
}
