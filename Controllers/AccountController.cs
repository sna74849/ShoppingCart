using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Services;
using ShoppingCart.Models.ViewModels;

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
            HttpContext.Session.Clear();
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginCheck([Bind] LoginViewModel loginVm)
        {
            try
            {
                if (!ModelState.IsValid) return View("Login");

                var customerEty = service.Login(loginVm.CustomerId, loginVm.Password);
                if (customerEty == null)
                {
                    ModelState.AddModelError("", "IDかパスワードが間違っています。");
                    return View("Login");
                }
                else
                {
                    HttpContext.Session.SetString("customerId", customerEty.CustomerId);
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
