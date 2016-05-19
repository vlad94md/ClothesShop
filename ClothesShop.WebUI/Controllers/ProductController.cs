using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ClothesShop.Domain.Abstract;
using ClothesShop.Domain.Entities;
using ClothesShop.WebUI.Models;

namespace ClothesShop.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IShopRepository repository;
        public int pageSize = 6;

        public ProductController(IShopRepository repos)
        {
            repository = repos;
        }

        public ViewResult Item(int Id)
        {
            var currProduct = repository.Products.FirstOrDefault(x => x.Id == Id);

            var currUser = (User)System.Web.HttpContext.Current.Session["user"];

            if (currUser != null)
            {
                var product =
                    repository.Purchases.FirstOrDefault(x => x.UserName == currUser.Username && x.ProductId == Id);

                if (product != null)
                {
                    ViewBag.WasBought = true;
                }
            }

            var reviews =
                repository.Reviews.Where(x => x.ProductId == Id)
                    .OrderByDescending(x => x.Date).ToList();

            //double rate = Convert.ToDouble(reviews.Sum(x => x.Rate)) / Convert.ToDouble(reviews.Count);
            //currProduct.Rate = Math.Round(rate, 1);


            ItemViewModel model = new ItemViewModel() {Item = currProduct, Reviews = reviews};

            return View(model);
        }

        [HttpPost]
        public ViewResult Item(Review review)
        {
            var currUser = (User)System.Web.HttpContext.Current.Session["user"];

            review.Author = currUser.Username;
            review.Date = DateTime.Now;

            repository.SaveReview(review);

            var currProduct = repository.Products.FirstOrDefault(x => x.Id == review.ProductId);

            if (currUser != null)
            {

                ViewBag.WasBought = true;
            }

            var reviews =
                repository.Reviews.Where(x => x.ProductId == review.ProductId)
                    .OrderByDescending(x => x.Date).ToList();

            double rate = Convert.ToDouble(reviews.Sum(x => x.Rate)) / Convert.ToDouble(reviews.Count);
            currProduct.Rate = Math.Round(rate, 1);
            currProduct.ReviewsNumber = reviews.Count;

            repository.SaveProduct(currProduct);

            ItemViewModel model = new ItemViewModel() { Item = currProduct, Reviews = reviews };

            return View(model);
        }

        public ViewResult Search(string category, int page = 1, string name = "")
        {
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


            List<ItemViewModel> items = new List<ItemViewModel>();
            foreach (var item in products)
            {
                ItemViewModel modelItem = new ItemViewModel();
                modelItem.Item = item;
                modelItem.ReviewsCount = repository.Reviews.Count(x => x.ProductId == item.Id);

                items.Add(modelItem);
            }

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

            var products = repository.Products
                .Where(p => category == null || p.Category == category)
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
                    TotalItems = category == null ?
                    repository.Products.Count() :
                    repository.Products.Where(product => product.Category == category).Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }

        public ViewResult Recommend(int page = 1)
        { 
            string reccomendedCategory = GetReccommendedCategory();

            var products = repository.Products
            .Where(p => reccomendedCategory == null || p.Category == reccomendedCategory)
            .OrderBy(Product => Product.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = products,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = reccomendedCategory == null ?
                    repository.Products.Count() :
                    repository.Products.Where(product => product.Category == reccomendedCategory).Count()
                },
                CurrentCategory = reccomendedCategory
            };
            return View(model);
        }

        private string GetReccommendedCategory()
        {
            var currUser = (User)System.Web.HttpContext.Current.Session["user"];

            var itemsInHistory = repository.Purchases.Where(x => x.UserName == currUser.Username).ToList();

            List<Product> productsInHistory = new List<Product>();
            foreach (var prod in itemsInHistory)
            {
                var product = repository.Products.FirstOrDefault(x => x.Id == prod.ProductId);

                if (product != null)
                    productsInHistory.Add(product);
            }

            List<string> categoriesInHistory = new List<string>();
            foreach (var item in productsInHistory)
            {
                if(!categoriesInHistory.Contains(item.Category))
                    categoriesInHistory.Add(item.Category);
            }

            string mostRepeatableCategory = null;
            int counter = 0;

            foreach (var category in categoriesInHistory)
            {
                var count = productsInHistory.Count(x => x.Category == category);

                if (count > counter)
                {
                    counter = count;
                    mostRepeatableCategory = category;
                }
            }

            return mostRepeatableCategory;
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