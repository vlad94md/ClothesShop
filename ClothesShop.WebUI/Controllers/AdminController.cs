using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothesShop.Domain.Abstract;
using ClothesShop.Domain.Entities;

namespace ClothesShop.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Edit(int id)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.Id == id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("Changes in item \"{0}\" have been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Product deletedGame = repository.DeleteProduct(Id);
            if (deletedGame != null)
            {
                TempData["message"] = string.Format("Item \"{0}\" has been deleted",
                    deletedGame.Name);
            }
            return RedirectToAction("Index");
        }

        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public string SetupDatabase()
        {
            repository.SaveProduct(new Product()
            {
                Category = "Shoes",
                Id = 0,
                Name = "Red boots",
                Description = "Very fancy and nice female red shoes. Very comfortable and easy to wear",
                Price = 23
            });

            repository.SaveProduct(new Product()
            {
                Category = "Shoes",
                Id = 0, Name = "Brown boots",
                Description = "Some meaningfull description",
                Price = 21
            });

            repository.SaveProduct(new Product()
            {
                Category = "Shoes",
                Id = 0,
                Name = "White Shoes",
                Description = "Some meaningfull description",
                Price = 17
            });

            repository.SaveProduct(new Product()
            {
                Category = "Shoes",
                Id = 0,
                Name = "White Sneakers",
                Description = "Some meaningfull description",
                Price = 41
            });

            repository.SaveProduct(new Product()
            {
                Category = "Shirts",
                Id = 0,
                Name = "White T-Shirt",
                Description = "Some meaningfull description",
                Price = 32
            });

            repository.SaveProduct(new Product()
            {
                Category = "Jeans",
                Id = 0,
                Name = "Blue Jeans",
                Description = "Some meaningfull description",
                Price = 49
            });

            repository.SaveProduct(new Product()
            {
                Category = "Cap",
                Id = 0,
                Name = "Chicago Bulls Cap",
                Description = "Some meaningfull description",
                Price = 11
            });

            repository.SaveProduct(new Product()
            {
                Category = "Shirts",
                Id = 0,
                Name = "Rock black Shirt",
                Description = "Some meaningfull description",
                Price = 35
            });

            return "Db was setuped";
        }
    }
}