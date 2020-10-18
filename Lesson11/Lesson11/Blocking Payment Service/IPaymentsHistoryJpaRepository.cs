using System.Collections.Generic;

namespace Lesson11.Blocking_Payment_Service
{
    public interface IPaymentsHistoryJpaRepository
    {
        List<Payment> FindAllByUserId(string userId);
    }
}
