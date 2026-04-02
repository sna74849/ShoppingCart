using DBManager.Framework;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Actions
{
    /// <summary>
    /// 顧客情報を読み取るアクションを表します。
    /// </summary>
    /// <remarks>
    /// 指定された顧客IDとパスワードをもとに、
    /// データソースから該当する顧客情報を取得します。
    /// 主にログイン認証や顧客情報参照処理で利用されます。
    /// </remarks>
    /// <param name="customerId">顧客ID</param>
    /// <param name="password">顧客のパスワード</param>
    /// <param name="customerDao">顧客データ取得用のデータアクセスオブジェクト</param>
    public class CustomerReadAction(string customerId, string password, CustomerDao customerDao) : IReadAction<CustomerEntity>
    {
        /// <summary>
        /// 顧客情報取得用のDAO
        /// </summary>
        private readonly IReadableDao<CustomerEntity> _customerDao = customerDao;

        /// <summary>
        /// 顧客情報を取得します。
        /// </summary>
        /// <returns>
        /// 該当する顧客が存在する場合は <see cref="CustomerEntity"/> を返します。
        /// 存在しない場合は <c>null</c> を返します。
        /// </returns>
        public CustomerEntity? ExecuteQuery()
        {
            return _customerDao.Fetch(customerId, password);
        }
    }
}
