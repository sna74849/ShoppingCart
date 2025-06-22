using DynamicDll.Db;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Dao;
using ShoppingCart.Models.Entity;

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

            return View();
        }

        public IActionResult LoginCheck(string customerId, string password)
        {
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
        }
    }
}
