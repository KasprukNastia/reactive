using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Lesson11.Math_in_the_Store
{
    public class Order
    {
		private readonly ProductsCatalog _productsCatalog;

		public string Id { get;  }
		public  string UserId { get; }
		public IEnumerable<string> ProductsIds { get; }	

		public Order(string id, 
			string userId, 
			IEnumerable<string> productsIds, 
			ProductsCatalog productsCatalog)
		{
			Id = id ?? throw new ArgumentNullException(nameof(id));
			UserId = userId ?? throw new ArgumentNullException(nameof(userId));
			ProductsIds = productsIds ?? throw new ArgumentNullException(nameof(productsIds));
			_productsCatalog = productsCatalog ?? throw new ArgumentNullException(nameof(productsCatalog));
		}

		public IObservable<long> GetTotalPrice() => ProductsIds.ToObservable()
			.Select(id => _productsCatalog.FindById(id))
			.Sum(product => product.Price);
	}
}
