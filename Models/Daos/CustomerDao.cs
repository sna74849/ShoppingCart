using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    public class CustomerDao : BaseEntityDao<CustomerEntity>
    {
        protected override CustomerEntity? Fetch(params object[] pkeys) {
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
                            CustomerId = reader.GetNonNullString("customer_id"),
                            Email = reader.GetNonNullString("email")!,
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        protected override List<CustomerEntity> Find()
        {
            throw new NotImplementedException();
        }
        protected override List<CustomerEntity> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        protected override int Insert(CustomerEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override int Update(CustomerEntity entity)
        {
            throw new NotImplementedException();
        }
        protected override int Delete(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        protected override int Patch(object value, params object[] pkeys)
        {
            throw new NotImplementedException();
        }
    }
}
