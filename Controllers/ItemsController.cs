using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Services;

namespace ShoppingCart.Controllers
{
    public class ItemsController(ItemService dbService) : Controller
    {
        [HttpGet("/items")]
        public IActionResult Index()
        {
            try
            {
                return View(dbService.GetItemSalesStockList());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}
