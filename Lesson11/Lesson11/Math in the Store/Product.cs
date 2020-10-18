using System;

namespace Lesson11.Math_in_the_Store
{
    public class Product
    {
        public string Id { get; }
        public string Name { get; }
        public long Price { get; }

        public Product(string id, string name, long price)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price;
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (o == null || !(o is Product product)) return false;

            if (Price != product.Price) return false;
            if (Id != null ? !Id.Equals(product.Id) : product.Id != null) return false;
            return Name != null ? Name.Equals(product.Name) : product.Name == null;
        }

        public override int GetHashCode()
        {
            int result = Id != null ? Id.GetHashCode() : 0;
            result = 31 * result + (Name != null ? Name.GetHashCode() : 0);
            result = 31 * result + (int)(Price ^ (Price >> 32));
            return result;
        }
    }
}
