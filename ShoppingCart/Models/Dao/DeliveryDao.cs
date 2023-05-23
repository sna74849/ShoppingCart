using DynamicDll.Db;
using ShoppingCart.Models.Entity;
using System.Data.SqlClient;

namespace ShoppingCart.Models.Dao {
    public class DeliveryDao : BaseDao<DeliveryEntity> {
        public override int Delete(DeliveryEntity entity) {
            throw new NotImplementedException();
        }

        public override DeliveryEntity Find(params object[] pkeys) {
            throw new NotImplementedException();
        }

        public override int Insert(DeliveryEntity entity) {
            string query = @"INSERT INTO t_delivery(order_cd, branch_no, destination_no, create_at) ";
            query += "VALUES(@orderCd, @branchNo, @destinationNo, GETDATE())";

            using (SqlCommand cmd = new SqlCommand(query, con)) {
                cmd.Parameters.Add(new SqlParameter("@orderCd", System.Data.SqlDbType.Char)).Value = entity.OrderCd;
                cmd.Parameters.Add(new SqlParameter("@branchNo", System.Data.SqlDbType.Int)).Value = entity.BranchNo;
                cmd.Parameters.Add(new SqlParameter("@destinationNo", System.Data.SqlDbType.Int)).Value = entity.DestinationNo;

                cmd.Transaction = trn;

                return cmd.ExecuteNonQuery();
            }
        }

        public override int Update(DeliveryEntity entity) {
            throw new NotImplementedException();
        }
    }
}
