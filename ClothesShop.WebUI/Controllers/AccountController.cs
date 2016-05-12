using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothesShop.Domain.Abstract;
using ClothesShop.WebUI.Models;

namespace ClothesShop.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;
        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
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


        public ViewResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ViewResult SignIn(LoginViewModel user)
        {
            return View();
        }

        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Register(UserModel user)
        {
            return View();
        }
    }
}