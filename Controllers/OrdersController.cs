using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Exceptions;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{
    public class OrdersController(DatabaseService dbService) : Controller
    {
        [HttpGet("/orders/{orderCd}")]
        public IActionResult Index([FromRoute] string orderCd)
        {
            return View(dbService.Read(action: () => {
                return new OrderReadAction(orderCd).Execute();
            }));
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
                var orderWriteAction = new OrderWriteAction(destinationNo, cartItemDtoList, orderScheduledDeliveryViewModels);
                var orderCd = dbService.Write(orderWriteAction)!;
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
