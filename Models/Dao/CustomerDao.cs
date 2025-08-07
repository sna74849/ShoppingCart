<<<<<<< HEAD
﻿using ShoppingCart.Models.Entity;

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
=======
﻿using DynamicDll.Db;
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
>>>>>>> 91decb47f3a5019fbd405ce4ab88d86b6396da4d
            throw new NotImplementedException();
        }
    }
}
