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
    public class CartController : Controller
    {
        private IShopRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IShopRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart,int id, string returnUrl)
        {
            Product product = repository.Products
                    .FirstOrDefault(g => g.Id == id);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int id, string returnUrl)
        {
            Product product = repository.Products
                    .FirstOrDefault(g => g.Id == id);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        [HttpGet]
        public ViewResult Checkout()
        {
            ShippingDetails model = new ShippingDetails();
            
            var currUser = (User)System.Web.HttpContext.Current.Session["user"];
            if (currUser != null)
            {
                model.Name = currUser.Name;
                model.Number = currUser.Number;
                model.City = currUser.City;
                model.Line1 = currUser.Line1;
                model.Line2 = currUser.Line2;
                model.Country = currUser.Country;
                model.Country = currUser.Country;
            }

            return View(model);
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, but your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);

                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session["user"] != null)
                { 
                    var currUser = (User)System.Web.HttpContext.Current.Session["user"];

                    foreach (var line in cart.Lines)
                    {
                        repository.SavePurchase(new Purchase()
                        {
                            Date = DateTime.Now, ProductId = line.Item.Id, UserName = currUser.Username, Amount = line.Quantity
                        });
                    }
                }
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
    }
}
