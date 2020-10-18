using System;

namespace Lesson11.Distributed_Transactions
{
    public class ErrorResult : IResult
    {
		public Exception Error { get; }

		public long TransactionId => -1;

		public ErrorResult(Exception throwable)
		{
			Error = throwable;
		}
	}
}
