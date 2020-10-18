using System;

namespace Lesson11.Distributed_Transactions
{
    public interface IConnection<T>
    {
        IObservable<int> Close();

        IObservable<int> Rollback();

        IObservable<long> Write(IObservable<T> data);
    }
}
