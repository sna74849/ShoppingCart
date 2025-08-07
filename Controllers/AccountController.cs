using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Dao;


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
        }
    }
}
