using System;

namespace Lesson11.Distributed_Transactions
{
    public interface IResult
    {
        Exception Error { get; }
        long TransactionId { get; }
        IResult ErrorResult(Exception t) => new ErrorResult(t);
        IResult Ok(long transactionId) => new SuccessResult(transactionId);
    }
}
