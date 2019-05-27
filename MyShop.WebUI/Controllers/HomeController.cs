using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Product> _context;
        private IRepository<ProductCategory> _productCategories;

        public HomeController(IRepository<Product> context, IRepository<ProductCategory> productCategories)
        {
            _context = context;
            _productCategories = productCategories;
        }

        public ActionResult Index(string category=null)
        {
            var products = new List<Product>();
            var productCategories = _productCategories.Collection().ToList();

            if (category == null)
            {
                products = _context.Collection().ToList();
            }
            else
            {
                products = _context.Collection().Where(p => p.Category.Equals(category)).ToList();
            }

            ProductListViewModel model = new ProductListViewModel
            {
                Products = products,
                ProductCategories = productCategories
            };

            return View(model);
        }

        public ActionResult Details(string id)
        {
            var product = _context.Find(id);
            if (product == null)
                return HttpNotFound();
            else
                return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}