using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Lesson11.Blocking_Payment_Service
{
    public class BlockingPaymentsHistoryJpaRepository : IPaymentsHistoryJpaRepository
    {
        public List<Payment> FindAllByUserId(string userId)
        {
			try
			{
				ConnectionsPool.Instance.TryAcquire();
				var random = new Random();

				return Enumerable.Repeat(new Payment(), random.Next(0, 50)).ToObservable()
					.Delay(TimeSpan.FromMilliseconds(random.Next(5, 50)))
					.Collect(() => new Payment(), (newPayment, oldPayment) => oldPayment)
					.ToList();
			}
			finally
			{
				ConnectionsPool.Instance.Release();
			}
		}
    }
}
