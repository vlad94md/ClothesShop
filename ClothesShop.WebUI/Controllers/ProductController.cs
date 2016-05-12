using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothesShop.Domain.Abstract;
using ClothesShop.Domain.Entities;
using ClothesShop.WebUI.Models;

namespace ClothesShop.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int pageSize = 6;

        public ProductController(IProductRepository repos)
        {
            repository = repos;
        }

        public ViewResult Item(int Id)
        {
            var model = repository.Products.FirstOrDefault(x => x.Id == Id);

            return View(model);
        }


        public ViewResult Search(string category, int page = 1, string name = "")
        {
            //var model = repository.Products.Where(x => x.Name.Contains(name)).ToList();
            ViewBag.name = name;
            ViewBag.category = category;

            var products = String.IsNullOrEmpty(category)
                ? repository.Products
                    .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                    .OrderBy(Product => Product.Id)
                    .Skip((page - 1)*pageSize)
                    .Take(pageSize)
                : repository.Products.Where(x => x.Name.ToLower().Contains(name.ToLower()) && x.Category == category)
                    .OrderBy(Product => Product.Id)
                    .Skip((page - 1)*pageSize)
                    .Take(pageSize);

            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = products,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = String.IsNullOrEmpty(category) ?
                    repository.Products.Where(x => x.Name.ToLower().Contains(name.ToLower())).Count() :
                    repository.Products.Where(x => x.Name.ToLower().Contains(name.ToLower()) && x.Category == category).Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(Product => Product.Id)
                    .Skip((page - 1)*pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    repository.Products.Count() :
                    repository.Products.Where(product => product.Category == category).Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }

        public FileContentResult GetImage(int Id)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.Id == Id);

            if (product != null)
            {
                try
                {
                    return File(product.ImageData, product.ImageMimeType);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}