using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["Products"] as List<Product>;
            if (products == null)
                products = new List<Product>();
        }

        public void Commit()
        {
            cache["Products"] = products;
        }

        public void Insert(Product product)
        {
            products.Add(product);
        }

        public void Update(Product product)
        {
            var productFromMemory = products.Find(p => p.Id.Equals(product.Id));

            if (productFromMemory == null)
                throw new Exception("Product not found");
            else
                productFromMemory = product;
        }

        public Product Find(string id)
        {
            var product = products.Find(p => p.Id.Equals(id));

            if (product == null)
                throw new Exception("Product not found");
            else
                return product;
        }

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string id)
        {
            var product = products.Find(p => p.Id.Equals(id));

            if (product == null)
                throw new Exception("Product not found");
            else
                products.Remove(product);
        }

    }
}
