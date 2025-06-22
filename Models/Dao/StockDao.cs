using DynamicDll.Db;
using ShoppingCart.Models.Entity;
using System.Data.SqlClient;

namespace ShoppingCart.Models.Dao {
    public class StockDao : BaseDao<StockEntity> {
        public override int Delete(StockEntity entity) {
            throw new NotImplementedException();
        }

        public override StockEntity Find(params object[] pkeys) {
            throw new NotImplementedException();
        }

        public override int Insert(StockEntity entity) {
            throw new NotImplementedException();
        }

        public override int Update(StockEntity entity) {
            string query = @"UPDATE TOP(1) t_stock SET order_cd = @orderCd, branch_no = @branchNo ";
            query += "WHERE order_cd IS NULL AND jan_cd = @janCd";

            using (SqlCommand cmd = new SqlCommand(query, con)) {
                cmd.Parameters.Add(new SqlParameter("@janCd", System.Data.SqlDbType.Char)).Value = entity.JanCd;
                cmd.Parameters.Add(new SqlParameter("@orderCd", System.Data.SqlDbType.Char)).Value = entity.OrderCd;
                cmd.Parameters.Add(new SqlParameter("@branchNo", System.Data.SqlDbType.Int)).Value = entity.BranchNo;

                cmd.Transaction = trn;

                return cmd.ExecuteNonQuery();
            }
        }
    }
}
