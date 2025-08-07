<<<<<<< HEAD
﻿using ShoppingCart.Models.Entity;

namespace ShoppingCart.Models.Dao {
    public class StockDao : IBaseEntityDao<StockEntity> {

        public StockEntity Find(params object[] pkeys) {
            throw new NotImplementedException();
        }
        public List<StockEntity> Find()
        {
            throw new NotImplementedException();
        }

        public List<StockEntity> FindBy(params object[] pkeys)
        {
            string janCd = pkeys[0]?.ToString() ?? throw new ArgumentNullException("janCd is null");
            int qty = Convert.ToInt32(pkeys[1]);

            string query = $@"
                            SELECT 
                                TOP {qty} stock_no
                            FROM 
                                t_stock WITH (XLOCK, ROWLOCK)
                            WHERE 
                                order_cd IS NULL
                            AND
                                jan_cd = @janCd
                            ";

            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@janCd", janCd)
                .Build();
            {
                using var reader = cmd.ExecuteReader();
                {

                    List<StockEntity> stockNoList = new();
                    while (reader.Read())
                    {
                        stockNoList.Add(new StockEntity
                        {
                            StockNo = reader.GetInt("stock_no"),
                        });
                    }
                    return stockNoList;
                }
            }
        }

        public int Insert(StockEntity entity) {
            throw new NotImplementedException();
        }

        public int Update(StockEntity entity) 
        {
            string query = @"
                            UPDATE 
                                t_stock
                            SET 
                                order_cd = @orderCd,
                                updated_at = GETDATE()
                            WHERE 
                                stock_no = @stockNo";

            using  var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@orderCd", entity.OrderCd)
                .AddParameter("@stockNo", entity.StockNo)
                .Build();
            {
                return cmd.ExecuteNonQuery();
            }
        }
        public int Delete(StockEntity entity)
        {
            throw new NotImplementedException();
        }
=======
﻿using DynamicDll.Db;
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
>>>>>>> 91decb47f3a5019fbd405ce4ab88d86b6396da4d
    }
}
