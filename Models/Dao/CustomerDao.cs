using ShoppingCart.Models.Entity;

namespace ShoppingCart.Models.Dao
{
    public class CustomerDao : IBaseEntityDao<CustomerEntity> 
    {
        public CustomerEntity? Find(params object[] pkeys) {
            string query = @"
                            SELECT 
                                customer_id,
                                email 
                            FROM 
                                m_customer 
                            WHERE 
                                email = @email 
                            AND 
                                password = @password";

            using var com = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@email", pkeys[0])
                .AddParameter("@password", pkeys[1])
                .Build(); 
            {
                using var reader = com.ExecuteReader();
                {
                    if (reader.Read()) {
                        return new CustomerEntity
                        {
                            CustomerId = reader.GetString("customer_id"),
                            Email = reader.GetString("email")!,
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public List<CustomerEntity> Find()
        {
            throw new NotImplementedException();
        }

        public List<CustomerEntity> FindBy(params object[] pkeys)
        {
            throw new NotImplementedException();
        }
        public int Insert(CustomerEntity entity) 
        {
            throw new NotImplementedException();
        }

        public int Update(CustomerEntity entity) 
        {
            throw new NotImplementedException();
        }
        public int Delete(CustomerEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
