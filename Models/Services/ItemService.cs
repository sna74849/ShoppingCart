using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Models.Services
{
    public class ItemService(DatabaseFramework dbFramework, ItemSalesStockDao dao)
    {
        public List<ItemSalesStockDto>? GetItemSalesStockList()
        {
            return dbFramework.Execute(new ItemSalesStockListReadAction(dao));
        }
    }
}
