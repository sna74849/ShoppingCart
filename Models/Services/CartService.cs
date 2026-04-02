using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Exceptions;

namespace ShoppingCart.Models.Services
{
    public class CartService(DatabaseFramework dbFramework, ItemSalesStockDao dao)
    {
        public ItemSalesStockDto GetItem(string janCd)
        {
            return dbFramework.Execute(new ItemSalesStockReadAction(janCd, dao)) ?? throw new StockException("在庫がありません。");
        }
    }
}
