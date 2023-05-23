using DynamicDll.Db;
using ShoppingCart.Models.Dto;
using ShoppingCart.Models.Entity;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace ShoppingCart.Models.Dao {
    public class OrderHistoryDao : BaseDao<OrderHistoryDto> {

        public override int Delete(OrderHistoryDto entity) {
            throw new NotImplementedException();
        }

        public override OrderHistoryDto Find(params object[] pkeys) {
            throw new NotImplementedException();
        }

        public override int Insert(OrderHistoryDto entity) {
            throw new NotImplementedException();
        }

        public override int Update(OrderHistoryDto entity) {
            throw new NotImplementedException();
        }

        public List<OrderHistoryDto> FindAll(string orderCd) {
            List<OrderHistoryDto> orderHistoryDtos = new List<OrderHistoryDto>();

            string query = @"SELECT o.order_cd, o.branch_no, ";
            query += "(SELECT j.item_nm FROM m_jancode j WHERE j.jan_cd = ";
            query += "(SELECT s.jan_cd FROM m_salse_item s WHERE s.item_cd = o.item_cd ))";
            query += " AS item_nm";
            query += ", d.destination_no, d.name, d.postcode, d.address, d.phone, d.email";
            query += "FROM t_order o ";
            query += "INNER JOIN t_delivery ON o.order_cd = t_delivery.order_cd ";
            query += "INNER JOIN m_destination d ON d.destination_no = t_delivery.destination_no ";
            query += "WHERE o.order_cd =@orderCd";

            using (SqlCommand cmd = new SqlCommand(query, con)) {
                cmd.Transaction = trn;
                cmd.Parameters.Add(new SqlParameter("@orderCd", System.Data.SqlDbType.Char)).Value = orderCd;
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        OrderHistoryDto orderHistoryDto = new OrderHistoryDto();
                        orderCd = reader.GetString(0);
                        orderHistoryDto.DestinationNo = GetInt(reader["destination_no"]) ?? 0;
                        orderHistoryDto.Name = GetString(reader["name"]);
                        orderHistoryDto.Postcode = GetString(reader["postcode"]);
                        orderHistoryDto.Address = GetString(reader["address"]);
                        orderHistoryDto.Phone = GetString(reader["phone"]);
                        orderHistoryDto.Email = GetString(reader["email"]);
                        orderHistoryDtos.Add(orderHistoryDto);
                    }
                }
            }
            return orderHistoryDtos;
        }
    }
}
