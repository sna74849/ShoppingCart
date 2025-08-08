using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Dao;
using ShoppingCart.Models.Dto;

namespace ShoppingCart.Controllers
{
    public class ItemsController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                var itemSalesStockDtoList = new List<ItemSalesStockDto>();

                using (new ConnectionManager("Shopping"))
                {
                    return View(new ItemSalesStockDao().Find());
                }
            }
            catch(Exception e)
            {
                ViewData["stackTrace"] = e.StackTrace;
                ViewData["message"] = e.Message;

                return View("Error");
            }
        }
    }
}
