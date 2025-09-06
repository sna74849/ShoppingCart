using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    /// <summary>
    /// 受注ヘッダテーブル (t_order_header) にアクセスする DAO クラス。<br/>
    /// 受注ヘッダの登録・検索・更新・削除を行う機能を提供する。
    /// </summary>
    public class OrderHeaderDao : BaseEntityDao<OrderHeaderEntity>
    {
        /// <summary>
        /// 主キーを指定して受注ヘッダデータを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">検索キー。</param>
        /// <returns>対象の受注ヘッダエンティティ。</returns>
        protected override OrderHeaderEntity Fetch(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// すべての受注ヘッダデータを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <returns>受注ヘッダエンティティのリスト。</returns>
        protected override List<OrderHeaderEntity> Find()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して受注ヘッダデータのリストを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">検索キー。</param>
        /// <returns>受注ヘッダエンティティのリスト。</returns>
        protected override List<OrderHeaderEntity> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新しい受注ヘッダデータを登録する。
        /// </summary>
        /// <param name="entity">登録対象の <see cref="OrderHeaderEntity"/>。</param>
        /// <returns>挿入された行数。</returns>
        protected override int Insert(OrderHeaderEntity entity)
        {
            string query = @"
                            INSERT INTO 
                                t_order_header
                                (
                                    order_cd,
                                    destination_no
                                )
                            VALUES
                                (   
                                    @orderCd,   
                                    @destinationNo
                                )";
            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@orderCd", entity.OrderCd)
                .AddParameter("@destinationNo", entity.DestinationNo)
                .Build();
            {
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 既存の受注ヘッダデータを更新する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="entity">更新対象の <see cref="OrderHeaderEntity"/>。</param>
        /// <returns>更新された行数。</returns>
        protected override int Update(OrderHeaderEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して受注ヘッダデータを削除する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">削除対象を特定するキー。</param>
        /// <returns>削除された行数。</returns>
        protected override int Delete(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 部分的に受注ヘッダデータを更新する。<br/>
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
