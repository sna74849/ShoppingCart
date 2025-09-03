using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Dtos;
namespace ShoppingCart.Models.Daos
{
    public class ItemSalesStockDao : BaseDtoDao<ItemSalesStockDto>
    {

        /// <summary>
        /// JANコードに対応する販売商品の在庫データを取得する
        /// </summary>
        /// <param name="pkeys">JANコード</param>
        protected override ItemSalesStockDto? Fetch(params object[] pkeys) {
            string query = @"
                            SELECT 
                                jan_cd,
                                item_nm,
                                file_nm,
                                price,
                                qty
                            FROM 
                                v_item_sales_stock
                            WHERE
                                jan_cd = @janCd
                            ";

            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@janCd", pkeys[0])
                .Build();
            {
                using var reader = cmd.ExecuteReader();
                {
                    if (reader.Read())
                    {
                        return new ItemSalesStockDto
                        {
                            JanCd = reader.GetNonNullString("jan_cd"),
                            ItemNm = reader.GetNonNullString("item_nm"),
                            FileNm = reader.GetNonNullString("file_nm"),
                            Price = reader.GetNonNullInt("price"),
                            Qty = reader.GetNonNullInt("qty"),
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 在庫数込み商品販売リストを取得
        /// </summary>
        protected override List<ItemSalesStockDto> Find() {
            string query = @"
                            SELECT 
                                jan_cd,
                                item_nm,
                                file_nm,
                                price,
                                qty
                            FROM 
                                v_item_sales_stock
                            ";

            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .Build();
            {
                using var reader = cmd.ExecuteReader();
                {
                    List<ItemSalesStockDto> itemSalesStocksDto = [];
                    while (reader.Read())
                    {
                        itemSalesStocksDto.Add(new ItemSalesStockDto
                        {
                            JanCd = reader.GetNonNullString("jan_cd"),
                            ItemNm = reader.GetNonNullString("item_nm"),
                            FileNm = reader.GetNonNullString("file_nm"),
                            Price = reader.GetNonNullInt("price"),
                            Qty = reader.GetNonNullInt("qty"),
                        });
                    }
                    return itemSalesStocksDto;
                }
            }
        }

        protected override List<ItemSalesStockDto> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

    }
}
