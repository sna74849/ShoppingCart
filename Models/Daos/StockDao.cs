using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    public class StockDao : BaseEntityDao<StockEntity> {

        protected override StockEntity Fetch(params object[] pkeys) {
            throw new NotImplementedException();
        }
        protected override List<StockEntity> Find()
        {
            throw new NotImplementedException();
        }

        protected override List<StockEntity> Find(params object[] pkeys)
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
                            StockNo = reader.GetNonNullInt("stock_no"),
                        });
                    }
                    return stockNoList;
                }
            }
        }

        protected override int Insert(StockEntity entity) {
            throw new NotImplementedException();
        }

        protected override int Update(StockEntity entity) 
        {
            throw new NotImplementedException();
        }
        protected override int Delete(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        protected override int Patch(object value, params object[] pkeys)
        {
            string orderCd = value?.ToString() ?? throw new ArgumentNullException("orderCd is null");
            string stockNo = pkeys[0]?.ToString() ?? throw new ArgumentNullException("stockNo is null");

            string query = @"
                            UPDATE 
                                t_stock
                            SET 
                                order_cd = @orderCd,
                                updated_at = GETDATE()
                            WHERE 
                                stock_no = @stockNo";

            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@orderCd", orderCd)
                .AddParameter("@stockNo", stockNo)
                .Build();
            {
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
