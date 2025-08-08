using ShoppingCart.Models.Dto;

namespace ShoppingCart.Models.Dao
{
    public class ItemSalesStockDao : IBaseDtoDao<ItemSalesStockDto>
    {

        /// <summary>
        /// JANコードに対応する販売商品の在庫データを取得する
        /// </summary>
        /// <param name="pkeys">JANコード</param>
        public ItemSalesStockDto? Find(params object[] pkeys) {
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
                            JanCd = reader.GetString("jan_cd"),
                            ItemNm = reader.GetString("item_nm"),
                            FileNm = reader.GetString("file_nm"),
                            Price = reader.GetInt("price"),
                            Qty = reader.GetInt("qty"),
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
        public List<ItemSalesStockDto> Find() {
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
                    List<ItemSalesStockDto> itemSalesStocksDto = new();
                    while (reader.Read())
                    {
                        itemSalesStocksDto.Add(new ItemSalesStockDto
                        {
                            JanCd = reader.GetString("jan_cd"),
                            ItemNm = reader.GetString("item_nm"),
                            FileNm = reader.GetString("file_nm"),
                            Price = reader.GetInt("price"),
                            Qty = reader.GetInt("qty"),
                        });
                    }
                    return itemSalesStocksDto;
                }
            }
        }

        public List<ItemSalesStockDto> FindBy(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

    }
}
