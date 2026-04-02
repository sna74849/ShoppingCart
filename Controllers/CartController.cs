using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Exceptions;
using ShoppingCart.Models.Services;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{
    public class CartController(CartService dbService) : Controller
    {

        [HttpGet("/cart")]
        public IActionResult Index()
        {
            try
            {
                return View(HttpContext.Session.GetObject<List<CartItemViewModel>>("cart"));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        [HttpPost("/cart/items")]
        public IActionResult Create([FromForm] string janCd, [FromForm] int qty)
        {
            try
            {
                var cartItemVmList
                        = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? [];
                var cartItemVm = new CartItemViewModel
                {
                    Item = dbService.GetItem(janCd)
                };
                if (cartItemVmList.Find(it => it.Item.JanCd == janCd) == null)
                {
                    cartItemVm.InCartQty = qty;
                    cartItemVmList.Add(cartItemVm);
                }
                else
                {
                    cartItemVmList.Find(it => it.Item.JanCd == janCd)!.Item.Qty = qty;
                }
                    
                HttpContext.Session.SetObject<List<CartItemViewModel>>("cart", cartItemVmList);
                    
                TempData["count"] = cartItemVmList.Count;

                return RedirectToAction("Index","Items");// PRG法で二重送信を防ぐまた商品情報読込のためアクションメソッドを再実行する必要がある
            }
            catch (StockException e)
            {
                ViewData["message"] = e.Message;
                return RedirectToAction("Index", "Items");// PRG法で二重送信を防ぐまた商品情報読込のためアクションメソッドを再実行する必要がある
            }
            catch (Exception)
            {
                return RedirectToAction("Error");// PRG法で二重送信を防ぐ
            }
        }
        [HttpPut("/cart/items/{janCd}")]
        public IActionResult Update([FromRoute] string janCd, [FromForm] int qty)
        {
            try
            {
                var cartItemVmList
                    = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? throw new Exception();

                cartItemVmList.Find(it => it.Item.JanCd == janCd)!.InCartQty = qty;

                HttpContext.Session.SetObject<List<CartItemViewModel>>("cart", cartItemVmList);

                TempData["count"] = cartItemVmList.Count;
                return Ok(cartItemVmList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("/cart/items/{janCd}")]
        public IActionResult Delete([FromRoute] string janCd)
        {
            try
            {
                var cartItemVmList
                    = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? throw new Exception();

                cartItemVmList.Remove(cartItemVmList.Find(it => it.Item.JanCd == janCd) ?? throw new Exception());

                HttpContext.Session.SetObject<List<CartItemViewModel>>("cart", cartItemVmList);

                TempData["count"] = cartItemVmList.Count;
                return Ok(cartItemVmList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
