using System;

namespace Lesson11.Distributed_Transactions
{
    public class SuccessResult : IResult
    {
        public long TransactionId { get; }

        public Exception Error => null;

        public SuccessResult(long transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
