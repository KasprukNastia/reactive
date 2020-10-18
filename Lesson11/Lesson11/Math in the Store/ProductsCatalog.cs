using System;
using System.Collections.Generic;
using System.Linq;

namespace Lesson11.Math_in_the_Store
{
    public class ProductsCatalog
    {
        private static readonly Random random = new Random();
        private static readonly List<string> PRODUCTS_NAMES = new List<string> 
        {
            "Phone",
            "TV",
            "SonyPlayStation",
            "XBox",
            "Battery",
            "Lamp",
            "Toaster"
        };

        public Product FindById(string id)
        {
            return new Product(id,
                PRODUCTS_NAMES.ElementAt(random.Next(0, PRODUCTS_NAMES.Count)),
                random.Next(1, 10000)
            );
        }
    }
}
