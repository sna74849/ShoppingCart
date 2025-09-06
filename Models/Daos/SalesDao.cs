using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    /// <summary>
    /// 販売情報ビュー (v_sales) にアクセスする DAO クラス。<br/>
    /// 商品の販売コードや価格情報を取得する機能を提供する。
    /// </summary>
    public class SalesDao : BaseEntityDao<SalesEntity>
    {
        /// <summary>
        /// 指定された JAN コードに対応する販売情報を取得する。
        /// </summary>
        /// <param name="pkeys">
        /// 検索キー配列。  
        /// [0] = janCd (string)
        /// </param>
        /// <returns>
        /// 該当する <see cref="SalesEntity"/> を返す。  
        /// 見つからなかった場合は <c>null</c> を返す。
        /// </returns>
        protected override SalesEntity? Fetch(params object[] pkeys)
        {
            var janCd = pkeys[0].ToString() ?? throw new ArgumentNullException(nameof(pkeys), "[0] = janCd (string)");

            string query = @"
                            SELECT 
                                sales_cd, 
                                price
                            FROM 
                                v_sales
                            WHERE 
                                jan_cd = @JanCd";
            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@JanCd", janCd)
                .Build();
            {
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new SalesEntity
                    {
                        SalesCd = reader.GetNonNullString("sales_cd"),
                        Price = reader.GetNonNullInt("price"),
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// すべての販売情報を取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <returns>販売情報エンティティのリスト。</returns>
        protected override List<SalesEntity> Find()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 検索キーを指定して販売情報を取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">検索キー。</param>
        /// <returns>販売情報エンティティのリスト。</returns>
        protected override List<SalesEntity> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新しい販売情報を登録する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="t">登録対象の <see cref="SalesEntity"/>。</param>
        /// <returns>挿入された行数。</returns>
        protected override int Insert(SalesEntity t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 既存の販売情報を更新する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="t">更新対象の <see cref="SalesEntity"/>。</param>
        /// <returns>更新された行数。</returns>
        protected override int Update(SalesEntity t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して販売情報を削除する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">削除対象を特定するキー。</param>
        /// <returns>削除された行数。</returns>
        protected override int Delete(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 部分的に販売情報を更新する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="value">更新値。</param>
        /// <param name="pkeys">対象データを特定するキー。</param>
        /// <returns>更新された行数。</returns>
        protected override int Patch(object value, params object[] pkeys)
        {
            throw new NotImplementedException();
        }
    }
}
