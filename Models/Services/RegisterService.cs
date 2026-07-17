using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.Exceptions;

namespace ShoppingCart.Models.Services
{
    public class RegisterService(DatabaseFramework dbFramework, DestinationDao destinationDao, ItemSalesStockDao itemSalesStockDao)
    {
        public List<DestinationEntity>? GetDestinationList(string customerId)
        {
            return dbFramework.Execute(new DestinationReadAction(customerId, destinationDao));
        }

        public ItemSalesStockDto GetItem(string janCd)
        {
            return dbFramework.Execute(new ItemSalesStockReadAction(janCd, itemSalesStockDao)) ?? throw new StockException("在庫がありません。");
        }
    }
}
