using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Exceptions;
using ShoppingCart.Models.Services;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{
    public class OrdersController(OrderService service) : Controller
    {
        [HttpGet("/orders/{orderCd}")]
        public IActionResult Index([FromRoute] string orderCd)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerId")))
            {
                //View("../Account/Login")で指定するとブラウザに"Home"のパスが残る。
                return RedirectToAction("Login", "Account");
            }
            return View(service.GetOrder(orderCd, HttpContext.Session.GetString("customerId")!));
        }

        [HttpPost("/orders")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] OrderWriteViewModel orderWriteVm)
        {
            var cartItemDtoList
                = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? [];

            if (cartItemDtoList.Count == 0)
            {
                TempData["ToastMessage"] = "カートは既に空です。";
                TempData["ToastType"] = "warning";
                return RedirectToAction("Index", "Cart"); // カートが空の場合はカート画面を再読み込み
            }

            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerId")))
                {
                    //View("../Account/Login")で指定するとブラウザに"Home"のパスが残る。
                    return RedirectToAction("Login", "Account");
                }

                var orderCd = service.CreateOrder(orderWriteVm, cartItemDtoList);
                HttpContext.Session.Remove("cart");
                HttpContext.Session.Remove("count");
                return LocalRedirect($"/orders/{orderCd}");// PRG法で二重送信を防ぐ
            }
            catch (Exception e)
            {
                if (e.InnerException is OrderException orderEx)
                {
                    ViewData["message"] = orderEx.Message;
                    return View("NoStockError");
                }
                return View("Error");
            }
        }

    }
}
