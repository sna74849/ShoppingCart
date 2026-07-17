using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Services;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{
    public class RegisterController(RegisterService dbService) : Controller
    {
        [HttpGet("/register")]
        public IActionResult Index()
        {
            var cartItemVmList
                = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? [];
            if (cartItemVmList.Count == 0)
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
                    ViewBag.Items = new List<ItemSalesStockDto>();
                    cartItemVmList.ForEach(it => ViewBag.Items.Add(dbService.GetItem(it.JanCd)));
                    ViewBag.Destinations = dbService.GetDestinationList(HttpContext.Session.GetString("customerId")!);

                    return View(cartItemVmList);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}
