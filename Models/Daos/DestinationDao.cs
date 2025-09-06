using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    /// <summary>
    /// 配送先情報テーブル (m_destination) にアクセスするための DAO クラス。
    /// 顧客に紐づく配送先の検索や管理を担当する。
    /// </summary>
    public class DestinationDao : BaseEntityDao<DestinationEntity>
    {
        /// <summary>
        /// すべての配送先情報を取得する。
        /// 未実装。
        /// </summary>
        /// <returns>配送先エンティティのリスト。</returns>
        protected override List<DestinationEntity> Find()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定された顧客IDに紐づく配送先情報を取得する。
        /// <paramref name="pkeys"/> の 0 番目に顧客IDを渡すことで、
        /// m_destination テーブルから対象の配送先一覧を検索する。
        /// </summary>
        /// <param name="pkeys">
        /// 検索キー配列。  
        /// [0] = customerId (string)
        /// </param>
        /// <returns>
        /// 配送先情報 (<see cref="DestinationEntity"/>) のリスト。  
        /// 該当する配送先が存在しない場合は空のリストを返す。
        /// </returns>
        protected override List<DestinationEntity> Find(params object[] pkeys)
        {
            var customerId = pkeys[0].ToString() ?? throw new ArgumentNullException(nameof(pkeys), "[0] = customerId (string)");
            string query = @"
                            SELECT 
                                destination_no, 
                                name, 
                                postcode, 
                                address, 
                                phone 
                            FROM 
                                m_destination
                            WHERE 
                                customer_id = @customerId";

            using var com = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@customerId", customerId)
                .Build();
            {
                using var reader = com.ExecuteReader();
                {
                    List<DestinationEntity> destinationEntities = [];

                    while (reader.Read())
                    {
                        destinationEntities.Add(new DestinationEntity
                        {
                            DestinationNo = reader.GetNonNullInt("destination_no"),
                            Name = reader.GetNonNullString("name"),
                            Postcode = reader.GetNonNullString("postcode"),
                            Address = reader.GetNonNullString("address"),
                            Phone = reader.GetNonNullString("phone"),
                        });
                    }
                    return destinationEntities;
                }
            }
        }

        /// <summary>
        /// 新規に配送先情報を追加する。
        /// 未実装。
        /// </summary>
        /// <param name="t">追加する配送先エンティティ。</param>
        /// <returns>追加件数。</returns>
        protected override int Insert(DestinationEntity t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 配送先情報を更新する。
        /// 未実装。
        /// </summary>
        /// <param name="t">更新する配送先エンティティ。</param>
        /// <returns>更新件数。</returns>
        protected override int Update(DestinationEntity t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して配送先情報を削除する。
        /// 未実装。
        /// </summary>
        /// <param name="pkeys">削除対象の主キー。</param>
        /// <returns>削除件数。</returns>
        protected override int Delete(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 配送先情報の一部を更新する。
        /// 未実装。
        /// </summary>
        /// <param name="value">更新値。</param>
        /// <param name="pkeys">対象の主キー。</param>
        /// <returns>更新件数。</returns>
        protected override int Patch(object value, params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して単一の配送先情報を取得する。
        /// 未実装。
        /// </summary>
        /// <param name="pkeys">検索キー。</param>
        /// <returns>配送先エンティティ。存在しない場合は <c>null</c>。</returns>
        protected override DestinationEntity? Fetch(params object[] pkeys)
        {
            throw new NotImplementedException();
        }
    }
}
