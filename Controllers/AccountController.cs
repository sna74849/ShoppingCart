using Microsoft.AspNetCore.Mvc;
using DBManager.Framework;
using ShoppingCart.Models;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.Daos;

namespace ShoppingCart.Controllers
{
    public class AccountController(DatabaseService dbservice) : Controller
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
            return View();
        }

        public IActionResult LoginCheck(string customerId, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(password))
                {
                    ViewData["errorMessage"] = "IDとパスワードを入力してください。";
                    return View("Login");
                }
                var customerEty = dbservice.Read(action:() =>
                    {
                        IReadableDao<CustomerEntity> customerDao = new CustomerDao();
                        return customerDao.Fetch(customerId, password);
                    }
                );
                if (customerEty == null)
                {
                    ViewData["errorMessage"] = "IDかパスワードが間違っています。";
                    return View("Login");
                }
                else
                {
                    TempData.Add("customerId", customerEty.CustomerId);
                }
                
                return View("../Home/Index");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}
