using DBManager.Framework;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Actions
{
    /// <summary>
    /// 配送先情報を取得するアクションを表します。
    /// </summary>
    /// <remarks>
    /// 指定された顧客IDに紐づく配送先一覧を、
    /// データソースから取得します。
    /// 主に配送先選択画面や注文処理で利用されます。
    /// </remarks>
    /// <param name="customerId">配送先を取得する対象の顧客ID</param>
    /// <param name="destinationDao">配送先データ取得用のデータアクセスオブジェクト</param>
    public class DestinationReadAction(string customerId, DestinationDao destinationDao) : IReadAction<List<DestinationEntity>>
    {
        /// <summary>
        /// 配送先情報取得用のDAO
        /// </summary>
        private readonly IReadableDao<DestinationEntity> _destinationDao = destinationDao;

        /// <summary>
        /// 配送先一覧を取得します。
        /// </summary>
        /// <returns>
        /// 指定された顧客に紐づく <see cref="DestinationEntity"/> のリストを返します。
        /// 該当する配送先が存在しない場合は空のリストを返します。
        /// </returns>
        public List<DestinationEntity> ExecuteQuery()
        {
            return _destinationDao.Find(customerId);
        }
    }
}
