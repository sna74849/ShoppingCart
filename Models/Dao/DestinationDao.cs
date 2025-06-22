using DynamicDll.Db;
using ShoppingCart.Models.Dto;
using ShoppingCart.Models.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShoppingCart.Models.Dao {
    public class DestinationDao : BaseDao<DestinationEntity> {
        public override int Delete(DestinationEntity entity) {
            throw new NotImplementedException();
        }
        public override DestinationEntity Find(params object[] pkeys) {
            throw new NotImplementedException();
        }
        public override int Insert(DestinationEntity entity) {
            throw new NotImplementedException();
        }
        public override int Update(DestinationEntity entity) {
            throw new NotImplementedException();
        }
        public List<DestinationEntity> FindAll(string customerId) {
            string query = @"SELECT destination_no, name, postcode, address, phone, email FROM m_destination ";
            query += "WHERE customer_id = @customerId";

            List<DestinationEntity> destinationEntities = new List<DestinationEntity>();
            using (SqlCommand cmd = new SqlCommand(query, con)) {
                cmd.Transaction = trn;
                cmd.Parameters.Add(new SqlParameter("@customerId", System.Data.SqlDbType.VarChar)).Value = customerId;
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        DestinationEntity destinatioEntity = new DestinationEntity();
                        destinatioEntity.DestinationNo = GetInt(reader["destination_no"]) ?? 0;
                        destinatioEntity.Name = GetString(reader["name"]);
                        destinatioEntity.Postcode = GetString(reader["postcode"]);
                        destinatioEntity.Address = GetString(reader["address"]);
                        destinatioEntity.Phone = GetString(reader["phone"]);
                        destinatioEntity.Email = GetString(reader["email"]);
                        destinationEntities.Add(destinatioEntity);
                    }
                }
            }
            return destinationEntities;
        }
    }
}
