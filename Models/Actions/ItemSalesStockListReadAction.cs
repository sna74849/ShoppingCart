using DBManager.Framework;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Models.Actions
{
    /// <summary>
    /// 商品の販売情報および在庫情報の一覧を取得するアクションを表します。
    /// </summary>
    /// <remarks>
    /// データソースから全ての商品に関する販売情報と在庫情報を取得します。
    /// フィルタ条件は指定せず、全件取得を行います。
    /// 主に商品一覧画面や在庫確認処理で利用されます。
    /// </remarks>
    /// <param name="itemSalesStockDao">商品販売・在庫情報取得用のデータアクセスオブジェクト</param>
    public class ItemSalesStockListReadAction(ItemSalesStockDao itemSalesStockDao) : IReadAction<List<ItemSalesStockDto>>
    {
        /// <summary>
        /// 商品販売情報および在庫情報取得用のDAO
        /// </summary>
        private readonly IReadableDao<ItemSalesStockDto> _itemSalesStockDao = itemSalesStockDao;

        /// <summary>
        /// 商品の販売情報および在庫情報の一覧を取得します。
        /// </summary>
        /// <returns>
        /// <see cref="ItemSalesStockDto"/> のリストを返します。
        /// 該当データが存在しない場合は空のリストを返します。
        /// </returns>
        public List<ItemSalesStockDto> ExecuteQuery()
        {
            return _itemSalesStockDao.Find();
        }
    }
}