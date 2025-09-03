using DBManager.Framework;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.Exceptions;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{
    public class RegisterController(DatabaseService dbService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
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
                    object[] pkeys = { (string)TempData["customerId"]! };
                    ViewBag.Destinations = dbService!.Read(() => 
                    {
                        return destinationDao!.Find(pkeys);
                    });
                    
                    // ログアウト後ヒストリーバックによりキャッシュで再表示しないように再リクエストをレスポンスに設定
                    HttpContext.Response.Headers.Append("Cache-Control", "private, no-cache, no-store, must-revalidate, max-stale=0, post-check=0, pre-check=0");
                    HttpContext.Response.Headers.Append("Pragma", "no-cache");
                    HttpContext.Response.Headers.Append("Expires", "-1");
                    return View(HttpContext.Session.GetObject<List<CartItemViewModel>>("cart"));
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost("/register/orders{destinationNo}")]
        public IActionResult Create([FromRoute] int destinationNo, [Bind] List<OrderScheduledDeliveryViewModel> orderScheduledDeliveryViewModels)
        {
            try
            {
                var cartItemDtoList = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? throw new Exception();
                if (string.IsNullOrEmpty((string?)TempData["customerId"]))
                {
                    //View("../Account/Login")で指定するとブラウザに"Home"のパスが残る。
                    return RedirectToAction("Login", "Account");
                }

                HttpContext.Session.Clear();
                TempData.Remove("count");
                var orderWriteAction = new OrderWriteAction(destinationNo, cartItemDtoList, orderScheduledDeliveryViewModels);
                return View("Order", dbService.Write(orderWriteAction) ?? throw new OrderException("在庫情報が取得できませんでした。"));
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
