using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    /// <summary>
    /// 在庫テーブル (t_stock) にアクセスする DAO クラス。<br/>
    /// 在庫の取得、登録、更新、削除を行う機能を提供する。
    /// </summary>
    public class StockDao : BaseEntityDao<StockEntity>
    {

        /// <summary>
        /// 主キーを指定して単一の在庫データを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">検索キー。</param>
        /// <returns>対象の在庫エンティティ。</returns>
        protected override StockEntity Fetch(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// すべての在庫データを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <returns>在庫エンティティのリスト。</returns>
        protected override List<StockEntity> Find()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定されたJANコードに対応する未割り当て在庫を取得する。<br/>
        /// 在庫数は <paramref name="pkeys"/> の 1 番目の値で制限する。
        /// </summary>
        /// <param name="pkeys">
        /// 検索キー配列。<br/>
        /// [0] = janCd (string)<br/>
        /// [1] = 取得数 (int)
        /// </param>
        /// <returns>在庫エンティティのリスト。</returns>
        protected override List<StockEntity> Find(params object[] pkeys)
        {
            var janCd = pkeys[0].ToString() ?? throw new ArgumentNullException(nameof(pkeys), "[0] = janCd (string)");
            int qty = Convert.ToInt32(pkeys[1] ?? throw new ArgumentNullException(nameof(pkeys), "[0] = qty (intS)"));

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

        /// <summary>
        /// 新しい在庫データを登録する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="entity">登録対象の <see cref="StockEntity"/>。</param>
        /// <returns>挿入された行数。</returns>
        protected override int Insert(StockEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 既存の在庫データを更新する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="entity">更新対象の <see cref="StockEntity"/>。</param>
        /// <returns>更新された行数。</returns>
        protected override int Update(StockEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して在庫データを削除する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">削除対象を特定するキー。</param>
        /// <returns>削除された行数。</returns>
        protected override int Delete(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 在庫に注文コードを紐付ける更新処理を行う。<br/>
        /// 指定された stockNo の在庫に orderCd を設定し、更新日時を更新する。
        /// </summary>
        /// <param name="value">orderCd (string)。</param>
        /// <param name="pkeys">
        /// 更新対象のキー配列。<br/>
        /// [0] = stockNo (string)
        /// </param>
        /// <returns>更新された行数。</returns>
        protected override int Patch(object value, params object[] pkeys)
        {
            var orderCd = value.ToString() ?? throw new ArgumentNullException(nameof(value), "orderCd is null");
            var stockNo = pkeys[0].ToString() ?? throw new ArgumentNullException(nameof(pkeys),"[0] = stockNo (string)");

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
