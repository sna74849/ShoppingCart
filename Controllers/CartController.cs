using DBManager.Framework;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Exceptions;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{
    public class CartController(DatabaseService dbService) : Controller
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
            IReadableDao<ItemSalesStockDto> dao = new ItemSalesStockDao();
            try
            {
                var cartItemDtoList
                        = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? [];
                var cartItemDto = new CartItemViewModel
                {
                    Item = dbService.Read(action:() =>
                    {
                        return dao.Fetch(janCd);
                    }) ?? throw new StockException("在庫がありません。")
                };
                if (cartItemDtoList.Find(it => it.Item.JanCd == janCd) == null)
                {
                    cartItemDto.InCartQty = qty;
                    cartItemDtoList.Add(cartItemDto);
                }
                else
                {
                    cartItemDtoList.Find(it => it.Item.JanCd == janCd)!.Item.Qty = qty;
                }
                    
                HttpContext.Session.SetObject<List<CartItemViewModel>>("cart", cartItemDtoList);
                    
                TempData["count"] = cartItemDtoList.Count;

                return View("../Items/Index", dbService!.Read(action:() => {
                    return dao.Find();
                }));
            }
            catch (StockException e)
            {
                ViewData["message"] = e.Message;
                return View("../Items/Index", dbService!.Read(action:() => {
                    return dao.Find();
                }));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        [HttpPut("/cart/items/{janCd}")]
        public IActionResult Update([FromRoute] string janCd, [FromForm] int qty)
        {
            try
            {
                var cartItemDtoList
                    = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? throw new Exception();

                cartItemDtoList.Find(it => it.Item.JanCd == janCd)!.InCartQty = qty;

                HttpContext.Session.SetObject<List<CartItemViewModel>>("cart", cartItemDtoList);

                TempData["count"] = cartItemDtoList.Count;
                return Ok(cartItemDtoList);
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
                var cartItemDtoList
                    = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart") ?? throw new Exception();

                cartItemDtoList.Remove(cartItemDtoList.Find(it => it.Item.JanCd == janCd) ?? throw new Exception());

                HttpContext.Session.SetObject<List<CartItemViewModel>>("cart", cartItemDtoList);

                TempData["count"] = cartItemDtoList.Count;
                return Ok(cartItemDtoList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
