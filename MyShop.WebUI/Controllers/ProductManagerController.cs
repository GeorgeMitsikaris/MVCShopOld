using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        private ProductRepository _context;

        public ProductManagerController()
        {
            _context = new ProductRepository();
        }

        public ActionResult Index()
        {
            var products = _context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);
            else
            {
                _context.Insert(product);
                _context.Commit();
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Edit(string id)
        {
            var product = _context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var productFromMemory = _context.Find(product.Id);
            if (productFromMemory == null)
                return HttpNotFound();
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    productFromMemory.Name = product.Name;
                    productFromMemory.Description = product.Description;
                    productFromMemory.Image = product.Image;
                    productFromMemory.Price = product.Price;
                    productFromMemory.Category = product.Category;
                    _context.Commit();
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        public ActionResult Delete(string id)
        {
            var product = _context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost, ActionName(nameof(Delete))]
        public ActionResult ConfirmDelete(string id)
        {
            var product = _context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                _context.Delete(id);
                _context.Commit();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}