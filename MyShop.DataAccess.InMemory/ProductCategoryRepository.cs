using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cache["ProductCategories"] as List<ProductCategory>;
            if (productCategories == null)
                productCategories = new List<ProductCategory>();
        }

        public void Commit()
        {
            cache["ProductCategories"] = productCategories;
        }

        public void Insert(ProductCategory productCategory)
        {
            productCategories.Add(productCategory);
        }

        public void Update(ProductCategory productCategory)
        {
            var productCategoryFromMemory = productCategories.Find(p => p.Id.Equals(productCategory.Id));

            if (productCategoryFromMemory == null)
                throw new Exception("Product Category not found");
            else
                productCategoryFromMemory = productCategory;
        }

        public ProductCategory Find(string id)
        {
            var productCategory = productCategories.Find(p => p.Id.Equals(id));

            if (productCategory == null)
                throw new Exception("Product Category not found");
            else
                return productCategory;
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string id)
        {
            var productCategory = productCategories.Find(p => p.Id.Equals(id));

            if (productCategory == null)
                throw new Exception("ProductCategory not found");
            else
                productCategories.Remove(productCategory);
        }
    }
}
