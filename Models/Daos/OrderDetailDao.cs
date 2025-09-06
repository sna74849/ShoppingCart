using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    /// <summary>
    /// 受注明細テーブル (t_order_detail) にアクセスする DAO クラス。<br/>
    /// 受注明細の登録・検索・更新・削除を行う機能を提供する。
    /// </summary>
    public class OrderDetailDao : BaseEntityDao<OrderDetailEntity>
    {
        /// <summary>
        /// 主キーを指定して受注明細データを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">検索キー。</param>
        /// <returns>対象の受注明細エンティティ。</returns>
        protected override OrderDetailEntity Fetch(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// すべての受注明細データを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <returns>受注明細エンティティのリスト。</returns>
        protected override List<OrderDetailEntity> Find()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して受注明細データのリストを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">検索キー。</param>
        /// <returns>受注明細エンティティのリスト。</returns>
        protected override List<OrderDetailEntity> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新しい受注明細データを登録する。
        /// </summary>
        /// <param name="entity">登録対象の <see cref="OrderDetailEntity"/>。</param>
        /// <returns>挿入された行数。</returns>
        protected override int Insert(OrderDetailEntity entity)
        {
            string query = @"
                            INSERT INTO t_order_detail 
                            (
                                order_cd, 
                                sales_cd, 
                                seq_no, 
                                scheduled_delivery_at,
                                shipped_at,
                                cancelled_at
                            ) 
                            VALUES 
                            (          
                                @orderCd,
                                @salesCd,
                                @seqNo, 
                                @scheduledDeliveryAt,
                                @shippedAt,
                                @cancelledAt
                            )";

            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@orderCd", entity.OrderCd)
                .AddParameter("@salesCd", entity.SalesCd)
                .AddParameter("@seqNo", entity.SeqNo)
                .AddParameter("@scheduledDeliveryAt", entity.ScheduledDeliveryAt)
                .AddParameter("@shippedAt", entity.ShippedAt)
                .AddParameter("@cancelledAt", entity.CancelledAt)
                .Build();
            {
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 既存の受注明細データを更新する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="t">更新対象の <see cref="OrderDetailEntity"/>。</param>
        /// <returns>更新された行数。</returns>
        protected override int Update(OrderDetailEntity t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して受注明細データを削除する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">削除対象を特定するキー。</param>
        /// <returns>削除された行数。</returns>
        protected override int Delete(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 部分的に受注明細データを更新する。<br/>
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
