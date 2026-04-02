using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Services
{
    public class RegisterService(DatabaseFramework dbFramework, DestinationDao dao)
    {
        public List<DestinationEntity>? GetDestinationList(string customerId)
        {
            return dbFramework.Execute(new DestinationReadAction(customerId, dao));
        }
    }
}
