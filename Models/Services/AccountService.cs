using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Services
{
    public class AccountService(DatabaseFramework dbFramework, CustomerDao dao)
    {
        public CustomerEntity? Login(string customerId, string password)
        {
            return dbFramework.Execute(new CustomerReadAction(customerId, password, dao));
        }
    }
}
