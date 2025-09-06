using DBManager.Framework;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{
    public class RegisterController(DatabaseService dbService) : Controller
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
                IReadableDao<DestinationEntity> destinationDao = new DestinationDao();
                if (string.IsNullOrEmpty((string?)TempData["customerId"]))
                {
                    //View("../Account/Login")で指定するとブラウザに"Home"のパスが残る。
                    return RedirectToAction("Login", "Account");
                } 
                else
                {
                    ViewBag.Destinations = dbService!.Read(() => 
                    {
                        return destinationDao!.Find((string)TempData["customerId"]!);
                    });

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
