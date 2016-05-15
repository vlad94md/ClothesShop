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
    public class AccountController : Controller
    {
        private IShopRepository repository;
        IAuthProvider authProvider;

        public AccountController(IAuthProvider auth, IShopRepository repo)
        {
            authProvider = auth;
            repository = repo;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("", "Wrong username or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult Logout(string url)
        {
            System.Web.HttpContext.Current.Session["user"] = null;

            return RedirectToAction("List", "Product");
        }

        public ViewResult Details()
        {
            var currUser = (User)System.Web.HttpContext.Current.Session["user"];

            return View(currUser);
        }

        [HttpPost]
        public ViewResult Details(User user)
        {
            repository.SaveUser(user);

            System.Web.HttpContext.Current.Session["user"] = user;

            ViewBag.message = "Changes have been saved successfully";

            return View(user);
        }

        public ViewResult History()
        {
            var currUser = (User)System.Web.HttpContext.Current.Session["user"];

            var purchases = repository.Purchases.Where(x => x.UserName == currUser.Username).ToList();

            List<PurchaseViewModel> model = new List<PurchaseViewModel>();
            foreach (var item in purchases)
            {
                var productName = repository.Products.First(x => x.Id == item.ProductId).Name;

                model.Add(new PurchaseViewModel() {
                    Id = item.Id,
                    UserName = item.UserName,
                    ProductId = item.ProductId,
                    Date = item.Date,
                    ProductName = productName,
                    Amount = item.Amount
                });
            }

            return View(model);
        }

        public ViewResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userToSignIn = repository.Users.FirstOrDefault(x => x.Username == user.UserName && x.Password == user.Password);

                if (userToSignIn != null)
                {
                    System.Web.HttpContext.Current.Session["user"] = userToSignIn;
                    return RedirectToAction("List", "Product");
                }
                ModelState.AddModelError("", "Wrong username or password");
                return View();
            }
            else
            {
                return View();
            }
        }

        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var userWithSameUsername = repository.Users.FirstOrDefault(x => x.Username == user.Username);

                if (userWithSameUsername != null)
                {
                    ModelState.AddModelError("", "Username already exists");
                    return View();
                }

                repository.SaveUser(user);

                return View("Success");
            }
            else
            {
                return View();
            }
        }
    }
}