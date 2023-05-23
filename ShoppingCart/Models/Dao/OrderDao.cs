using DynamicDll.Db;
using ShoppingCart.Models.Entity;
using System.Data.SqlClient;

namespace ShoppingCart.Models.Dao {
    public class OrderDao : BaseDao<OrderEntity> {
        public override int Delete(OrderEntity entity) {
            throw new NotImplementedException();
        }

        public override OrderEntity Find(params object[] pkeys) {
            throw new NotImplementedException();
        }

        public override int Insert(OrderEntity entity) {
            string query = @"INSERT INTO t_order(order_cd, branch_no, customer_id, item_cd, creade_at)";
            query += "VALUES(@orderCd, @branchNo, @customerId, @itemCd, GETDATE())";

            using (SqlCommand cmd = new SqlCommand(query, con)) {
                cmd.Parameters.Add(new SqlParameter("@orderCd", System.Data.SqlDbType.Char)).Value = entity.OrderCd;
                cmd.Parameters.Add(new SqlParameter("@branchNo", System.Data.SqlDbType.Int)).Value = entity.BranchNo;
                cmd.Parameters.Add(new SqlParameter("@customerId", System.Data.SqlDbType.VarChar)).Value = entity.CustomerId;
                cmd.Parameters.Add(new SqlParameter("@itemCd", System.Data.SqlDbType.Char)).Value = entity.ItemCd;
                cmd.Transaction = trn;

                return cmd.ExecuteNonQuery();
            }
        }

        public override int Update(OrderEntity entity) {
            throw new NotImplementedException();
        }
    }
}
