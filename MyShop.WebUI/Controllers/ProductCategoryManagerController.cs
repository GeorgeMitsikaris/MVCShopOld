using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        private IRepository<ProductCategory> _context;        

        public ProductCategoryManagerController(IRepository<ProductCategory> context)
        {
            _context = context;
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
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
                return View(productCategory);
            else
            {
                _context.Insert(productCategory);
                _context.Commit();
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Edit(string id)
        {
            var productCategory = _context.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategory);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            var productCategoryFromMemory = _context.Find(productCategory.Id);
            if (productCategoryFromMemory == null)
                return HttpNotFound();
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCategory);
                }
                else
                {
                    productCategoryFromMemory.Category = productCategory.Category;
                    _context.Commit();
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        public ActionResult Delete(string id)
        {
            var productCategory = _context.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategory);
            }
        }

        [HttpPost, ActionName(nameof(Delete))]
        public ActionResult ConfirmDelete(string id)
        {
            var productCategory = _context.Find(id);
            if (productCategory == null)
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