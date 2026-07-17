using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Exceptions;
using ShoppingCart.Models.Services;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CartController(CartService dbService) : Controller
    {

        [HttpGet("/cart")]
        public IActionResult Index()
        {
            try
            {
                var cartItemVmList
                    = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? [];
                var itemSalseStockDtoList = new List<ItemSalesStockDto>();
                cartItemVmList.ForEach(it => itemSalseStockDtoList.Add(dbService.GetItem(it.JanCd)));

                ViewBag.Items = itemSalseStockDtoList;

                return View(cartItemVmList);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        [HttpPost("/cart/items")]
        public IActionResult Create([Bind] CartItemViewModel cartItemVm)
        {
            try
            {
                var cartItemVmList
                        = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? [];
                var itemSalseStockDto = dbService.GetItem(cartItemVm.JanCd);
    
                if (cartItemVmList.Find(it => it.JanCd == cartItemVm.JanCd) == null)
                {
                    // カートに入っていない場合は新規追加する
                    cartItemVmList.Add(cartItemVm);
                }
                else
                {
                    // すでにカートに入っている場合は数量を更新する(在庫数との整合性はView側でとる)
                    cartItemVmList.Find(it => it.JanCd == cartItemVm.JanCd)!.Qty = cartItemVm.Qty;
                }
                    
                HttpContext.Session.SetObject<List<CartItemViewModel>>("cart", cartItemVmList);
                    
                HttpContext.Session.SetInt32("count",cartItemVmList.Count);

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

                cartItemVmList.Find(it => it.JanCd == janCd)!.Qty = qty;

                HttpContext.Session.SetObject<List<CartItemViewModel>>("cart", cartItemVmList);

                HttpContext.Session.SetInt32("count", cartItemVmList.Count);
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

                cartItemVmList.Remove(cartItemVmList.Find(it => it.JanCd == janCd) ?? throw new Exception());

                HttpContext.Session.SetObject<List<CartItemViewModel>>("cart", cartItemVmList);

                HttpContext.Session.SetInt32("count", cartItemVmList.Count);
                return Ok(cartItemVmList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
