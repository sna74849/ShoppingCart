using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Services;

namespace ShoppingCart.Controllers
{
    public class AccountController(AccountService service) : Controller
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
            TempData.Remove("count");
            HttpContext.Session.Clear();
            return View("Login");
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

                var customerEty = service.Login(customerId, password);
                if (customerEty == null)
                {
                    ViewData["errorMessage"] = "IDかパスワードが間違っています。";
                    return View("Login");
                }
                else
                {
                    TempData.Add("customerId", customerEty.CustomerId);
                }
                return RedirectToAction("Index","Home");// PRG法で二重送信を防ぐ
            }
            catch (Exception)
            {
                return RedirectToAction("Error","Home");// PRG法で二重送信を防ぐ
            }
        }
    }
}
