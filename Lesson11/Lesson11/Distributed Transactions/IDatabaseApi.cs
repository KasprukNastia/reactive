using System;

namespace Lesson11.Distributed_Transactions
{
    public interface IDatabaseApi
    {
        IObservable<IConnection<T>> Open<T>();

        public IObservable<int> RollbackTransaction(long id);
    }
}
