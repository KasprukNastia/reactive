using System;
using System.Linq;
using System.Reactive.Linq;

namespace Lesson11.Math_in_the_Store
{
    public class UserActivityUtils
    {
        public static IObservable<Product> FindMostExpansivePurchase(
            IObservable<Order> ordersHistory,
            ProductsCatalog productsCatalog)
        {
            IObservable<Product> allOrdersProducts = 
                ordersHistory.SelectMany(order => order.ProductsIds.Select(id => productsCatalog.FindById(id)));

            IObservable<long> maxProductPrice = allOrdersProducts.Max(price => price.Price);

            return maxProductPrice.SelectMany(price => allOrdersProducts.Where(product => product.Price == price));
        }
    }
}
