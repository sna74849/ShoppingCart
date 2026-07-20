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
                TempData["ToastMessage"] = "カートは既に空です。";
                TempData["ToastType"] = "warning";
                return RedirectToAction("Index", "Cart"); // カートが空の場合はカート画面にリダイレクト
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
                    if (dbService.TryGetItems(cartItemVmList, out List<ItemSalesStockDto> itemSalesStockDtoList))
                    {
                        ViewBag.Items = itemSalesStockDtoList;
                    }
                    else 
                    {
                        return RedirectToAction("Index", "Cart"); // カート内の商品が存在しない場合はカート画面にリダイレクト
                    }
                    
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
