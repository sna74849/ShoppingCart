using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Models.Daos
{
    /// <summary>
    /// 販売商品在庫ビュー (v_item_sales_stock) にアクセスする DAO クラス。<br/>
    /// 商品情報と在庫数をまとめて取得する機能を提供する。
    /// </summary>
    public class ItemSalesStockDao : BaseDtoDao<ItemSalesStockDto>
    {
        /// <summary>
        /// 指定された JAN コードに対応する販売商品の在庫データを取得する。<br/>
        /// 主に商品詳細画面やカート投入時の在庫確認に利用される。
        /// </summary>
        /// <param name="pkeys">
        /// 検索キー配列。  
        /// [0] = janCd (string)
        /// </param>
        /// <returns>
        /// 該当する <see cref="ItemSalesStockDto"/> を返す。  
        /// 見つからなかった場合は <c>null</c> を返す。
        /// </returns>
        protected override ItemSalesStockDto? Fetch(params object[] pkeys)
        {
            var janCd = pkeys[0].ToString() ?? throw new ArgumentNullException(nameof(pkeys), "[0] = janCd (string)");
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
                .AddParameter("@janCd", janCd)
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
        /// 在庫数を含めたすべての商品販売データを取得する。<br/>
        /// 商品一覧画面などで利用される。
        /// </summary>
        /// <returns>
        /// <see cref="ItemSalesStockDto"/> のリスト。<br/>
        /// 該当データが存在しない場合は空のリストを返す。
        /// </returns>
        protected override List<ItemSalesStockDto> Find()
        {
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

        /// <summary>
        /// 検索キーを指定して商品販売在庫データを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">検索キー。</param>
        /// <returns>在庫情報リスト。</returns>
        protected override List<ItemSalesStockDto> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }
    }
}
