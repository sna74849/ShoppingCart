<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Dao;

=======
﻿using DynamicDll.Db;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Dao;
using ShoppingCart.Models.Entity;
>>>>>>> 91decb47f3a5019fbd405ce4ab88d86b6396da4d

namespace ShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            TempData.Remove("customerId");
            HttpContext.Session.Clear();

<<<<<<< HEAD

=======
>>>>>>> 91decb47f3a5019fbd405ce4ab88d86b6396da4d
            return View();
        }

        public IActionResult LoginCheck(string customerId, string password)
        {
<<<<<<< HEAD
            try
            {
                if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(password))
                {
                    ViewData["errorMessage"] = "IDとパスワードを入力してください。";
                    return View("Login");
                }
                using (new ConnectionManager("shopping"))
                {
                    var customerEty = new CustomerDao().Find(customerId, password);
                    if (customerEty == null)
                    {
                        ViewData["errorMessage"] = "IDかパスワードが間違っています。";
                        return View("Login");
                    }
                    else
                    {
                        TempData.Add("customerId", customerEty.CustomerId);
                    }
                }
                return View("../Home/Index");
            }
            catch (Exception e)
            {
                ViewData["stackTrace"] = e.StackTrace;
                ViewData["message"] = e.Message;

                return View("Error");
            }
=======
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(password))
            {
                ViewData["errorMessage"] = "IDとパスワードを入力してください。";
                return View("Login");
            }
            using (TranMng mng = TranMng.BeginTransaction("Shopping"))
            {
                CustomerDao customerDao = new CustomerDao();
                CustomerEntity customerEntity = customerDao.Find(customerId, password);
                if (customerEntity == null)
                {
                    ViewData["errorMessage"] = "IDかパスワードが間違っています。";
                    return View("Login");
                }
                else
                {
                    TempData.Add("customerId", customerEntity.CustomerId);
                }
            }
            return View("../Home/Index");
>>>>>>> 91decb47f3a5019fbd405ce4ab88d86b6396da4d
        }
    }
}
