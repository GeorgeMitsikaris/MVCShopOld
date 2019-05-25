using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModel;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        private IRepository<Product> _context;
        private IRepository<ProductCategory> _productCategories;

        public ProductManagerController(IRepository<Product> context, IRepository<ProductCategory> productCategories)
        {
            _context = context;
            _productCategories = productCategories;
        }

        public ActionResult Index()
        {
            var products = _context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel();
            productManagerViewModel.Product = new Product();
            productManagerViewModel.ProductCategories = _productCategories.Collection();
            return View(productManagerViewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(product);
            else
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(@"/Images/" + product.Image));
                }

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
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = _productCategories.Collection();
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase file)
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
                    if (file!=null)
                    {
                        productFromMemory.Image = product.Id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath(@"/Images/" + productFromMemory.Image));
                    }

                    productFromMemory.Name = product.Name;
                    productFromMemory.Description = product.Description;
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