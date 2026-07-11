using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Services;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{
    public class RegisterController(RegisterService dbService) : Controller
    {
        [HttpGet("/register")]
        public IActionResult Index()
        {
            var cartItemDtoList
                = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? [];
            if (cartItemDtoList.Count == 0)
            {
                return View("../Cart/Index");
            }
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerId")))
                {
                    //View("../Account/Login")で指定するとブラウザに"Home"のパスが残る。
                    return RedirectToAction("Login", "Account");
                } 
                else
                {
                    ViewBag.Destinations = dbService.GetDestinationList(HttpContext.Session.GetString("customerId")!);

                    return View(cartItemDtoList);
                    
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}
