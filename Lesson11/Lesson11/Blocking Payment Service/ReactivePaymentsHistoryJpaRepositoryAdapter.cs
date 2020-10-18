using System;

namespace Lesson11.Blocking_Payment_Service
{
    public class ReactivePaymentsHistoryJpaRepositoryAdapter : IPaymentsHistoryReactiveJpaRepository
    {
        private readonly IPaymentsHistoryJpaRepository _repository;

	    public ReactivePaymentsHistoryJpaRepositoryAdapter(
            IPaymentsHistoryJpaRepository repository)
        {
            _repository = repository;
        }

        public IObservable<Payment> FindAllByUserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
