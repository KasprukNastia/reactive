using System;

namespace Lesson11.Blocking_Payment_Service
{
    public interface IPaymentsHistoryReactiveJpaRepository
    {
        IObservable<Payment> FindAllByUserId(string userId);
    }
}
