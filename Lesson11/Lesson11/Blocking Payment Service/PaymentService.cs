﻿using System;
using System.Reactive.Linq;

namespace Lesson11.Blocking_Payment_Service
{
    /**
 * Description: You have got a task to integrate our Reactive Application with JDBC. The
 * problem with JDBC is that it is blocking one and as we might remember blocking I/O is
 * pure evil. Fortunately, there is a hack here. We may rely on Cached ThreadPool and
 * execute each blocking operation in the separate Thread. It might help us when the
 * number of queries is small. However, when the number of queries become higher than
 * usual, we may get into the troubles. As we might remember, most of the Database has
 * connections-pool. That connections-pool is limited to prevent Database overwhelming. In
 * that case, if our application creates more Threads than available connections in the
 * connection-pool, then the application will be overwhelmed by redundant Thread. Hence
 * additional memory will be used.
 * <p>
 * As great developers, we have to tackle that noisy issue. To avoid that insufficient
 * wasting we have to provide adaptation of {@link BlockingPaymentsHistoryJpaRepository}
 * using {@link ReactivePaymentsHistoryJpaRepositoryAdapter} and custom Scheduler service.
 * In turn, you have to care about connections-pool limitations so your application will
 * not overwhelm it by redundant calls.
 */
    public class PaymentService
    {
		private readonly IPaymentsHistoryReactiveJpaRepository _repository;

		public PaymentService()
		{
			_repository =
					new ReactivePaymentsHistoryJpaRepositoryAdapter(new BlockingPaymentsHistoryJpaRepository()); // REPLACE
		}

		public IObservable<Payment> FindPayments(IObservable<string> userIds) =>
			userIds.SelectMany(id => _repository.FindAllByUserId(id));
	}
}
