using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
    {
        IRepository<Product> _productContext;
        IRepository<Basket> _basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> productContext, IRepository<Basket> basketContext)
        {
            _productContext = productContext;
            _basketContext = basketContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = _basketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            _basketContext.Insert(basket);
            _basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddHours(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);

            BasketItem basketItem = basket.BasketItems.FirstOrDefault(b => b.ProductId.Equals(productId));

            if (basketItem == null)
            {
                basketItem = new BasketItem
                {
                    ProductId = productId,
                    BasketId = basket.Id,
                    Quantity = 1
                };
                basket.BasketItems.Add(basketItem);
            }

            else
            {
                basketItem.Quantity++;
            }

            _basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, int itemId)
        {
            Basket basket = GetBasket(httpContext, true);

            BasketItem basketItem = basket.BasketItems.FirstOrDefault(bi => bi.Id.Equals(itemId));

            if (basketItem != null)
            {
                basket.BasketItems.Remove(basketItem);
                _basketContext.Commit();
            }
        }
    }
}
