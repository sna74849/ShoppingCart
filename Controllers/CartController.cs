using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Dao;
using ShoppingCart.Models.Dto;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        [HttpGet("/cart")]
        public IActionResult Index()
        {
            try
            {
                return View(HttpContext.Session.GetObject<List<CartItemDto>>("cart"));
            }
            catch (Exception e)
            {
                ViewData["stackTrace"] = e.StackTrace;
                ViewData["message"] = e.Message;

                return View("Error");
            }
        }
        [HttpPost("/cart/items")]
        public IActionResult Create(string janCd, int qty)
        {
            try
            {
                var cartItemDtoList
                        = HttpContext.Session.GetObject<List<CartItemDto>>("cart") ?? new List<CartItemDto>();
                using (new ConnectionManager("Shopping"))
                {

                    var cartItemDto = new CartItemDto();
                    cartItemDto.item = new ItemSalesStockDao().Find(janCd) ?? throw new Exception("No stock");

                    if (cartItemDtoList.Find(it => it.item.JanCd == janCd) == null)
                    {
                        cartItemDto.InCartQty = qty;
                        cartItemDtoList.Add(cartItemDto);
                    }
                    else
                    {
                        cartItemDtoList.Find(it => it.item.JanCd == janCd)!.item.Qty = qty;
                    }
                    
                    HttpContext.Session.SetObject<List<CartItemDto>>("cart", cartItemDtoList);
                    
                    TempData["count"] = cartItemDtoList.Count;

                    return View("../Items/Index", new ItemSalesStockDao().Find());
                }
            }
            catch (Exception e)
            {
                ViewData["stackTrace"] = e.StackTrace;
                ViewData["message"] = e.Message;

                return View("Error");
            }
        }
        [HttpPut("/cart/items/{janCd}")]
        public IActionResult Update([FromRoute] string janCd, [FromForm] int qty)
        {
            try
            {
                var cartItemDtoList
                    = HttpContext.Session.GetObject<List<CartItemDto>>("cart") ?? throw new Exception();

                cartItemDtoList.Find(it => it.item.JanCd == janCd)!.InCartQty = qty;

                HttpContext.Session.SetObject<List<CartItemDto>>("cart", cartItemDtoList);

                TempData["count"] = cartItemDtoList.Count;

                return View("Index", cartItemDtoList);
            }
            catch (Exception e)
            {
                ViewData["stackTrace"] = e.StackTrace;
                ViewData["message"] = e.Message;

                return View("Error");
            }
        }
        [HttpDelete("/cart/items/{janCd}")]
        public IActionResult Delete([FromRoute] string janCd)
        {
            try
            {
                var cartItemDtoList
                    = HttpContext.Session.GetObject<List<CartItemDto>>("cart") ?? throw new Exception();

                cartItemDtoList.Remove(cartItemDtoList.Find(it => it.item.JanCd == janCd) ?? throw new Exception());

                HttpContext.Session.SetObject<List<CartItemDto>>("cart", cartItemDtoList);

                TempData["count"] = cartItemDtoList.Count;

                return View("Index", cartItemDtoList);
            }
            catch (Exception e)
            {
                ViewData["stackTrace"] = e.StackTrace;
                ViewData["message"] = e.Message;

                return View("Error");
            }
        }
    }
}
