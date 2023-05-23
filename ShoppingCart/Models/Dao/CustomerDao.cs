using DynamicDll.Db;
using ShoppingCart.Models.Dto;
using ShoppingCart.Models.Entity;
using System.Data.SqlClient;

namespace ShoppingCart.Models.Dao {
    public class CustomerDao : BaseDao<CustomerEntity> {
        public override int Delete(CustomerEntity entity) {
            throw new NotImplementedException();
        }

        public override CustomerEntity Find(params object[] pkeys) {
            string query = @"SELECT customer_id FROM m_customer WHERE customer_id = @customerId AND password = @password";

            CustomerEntity customerEntity = new CustomerEntity();
            using (SqlCommand cmd = new SqlCommand(query, con)) {
                cmd.Transaction = trn;
                cmd.Parameters.Add(new SqlParameter("@customerId", System.Data.SqlDbType.VarChar)).Value = pkeys[0];
                cmd.Parameters.Add(new SqlParameter("@password", System.Data.SqlDbType.VarChar)).Value = pkeys[1];
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    reader.Read();
                    if (reader.HasRows) {
                        customerEntity.CustomerId = GetString(reader["customer_id"]);
                    } else {
                        return null;
                    }
                    
                }
            }
            return customerEntity;
        }

        public override int Insert(CustomerEntity entity) {
            throw new NotImplementedException();
        }

        public override int Update(CustomerEntity entity) {
            throw new NotImplementedException();
        }
    }
}
