using DBManager.Framework;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Controllers
{
    public class ItemsController(DatabaseService dbService) : Controller
    {
        [HttpGet("/items")]
        public IActionResult Index()
        {
            IReadableDao<ItemSalesStockDto> dao = new ItemSalesStockDao();
            try
            {
                return View(dbService.Read(action:() => {
                    return dao.Find();
                }));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}
