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
            return View(service.GetOrder(orderCd));
        }

        [HttpPost("/orders/{destinationNo}")]
        public IActionResult Create([FromRoute] int destinationNo, [Bind] List<OrderScheduledDeliveryViewModel> orderScheduledDeliveryViewModels)
        {
            var cartItemDtoList
                = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? [];
            if (cartItemDtoList.Count == 0)
            {
                return View("../Cart/Index");
            }
            try
            {
                if (string.IsNullOrEmpty((string?)TempData.Peek("customerId")))// TempData["customerId"]だとリダイレクトで消えてしまう。
                {
                    //View("../Account/Login")で指定するとブラウザに"Home"のパスが残る。
                    return RedirectToAction("Login", "Account");
                }

                HttpContext.Session.Clear();
                TempData.Remove("count");
                var orderCd = service.CreateOrder(destinationNo, cartItemDtoList, orderScheduledDeliveryViewModels);
                return LocalRedirect($"/orders/{orderCd}");// PRG法で二重送信を防ぐ
            }
            catch (OrderException e)
            {
                ViewData["message"] = e.Message;
                return View("Error");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

    }
}
