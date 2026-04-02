using DBManager.Framework;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Models.Actions
{
    /// <summary>
    /// 商品の販売情報および在庫情報を単一取得するアクションを表します。
    /// </summary>
    /// <remarks>
    /// 指定されたJANコードをもとに、
    /// 対応する商品の販売情報および在庫情報をデータソースから取得します。
    /// 主に商品詳細表示や在庫確認処理で利用されます。
    /// </remarks>
    /// <param name="janCd">取得対象となる商品のJANコード</param>
    /// <param name="itemSalesStockDao">商品販売・在庫情報取得用のデータアクセスオブジェクト</param>
    public class ItemSalesStockReadAction(string janCd, ItemSalesStockDao itemSalesStockDao) : IReadAction<ItemSalesStockDto>
    {
        /// <summary>
        /// 商品販売情報および在庫情報取得用のDAO
        /// </summary>
        private readonly IReadableDao<ItemSalesStockDto> _itemSalesStockDao = itemSalesStockDao;

        /// <summary>
        /// 商品の販売情報および在庫情報を取得します。
        /// </summary>
        /// <returns>
        /// 該当する商品が存在する場合は <see cref="ItemSalesStockDto"/> を返します。
        /// 存在しない場合は <c>null</c> を返します。
        /// </returns>
        public ItemSalesStockDto? ExecuteQuery()
        {
            return _itemSalesStockDao.Fetch(janCd);
        }
    }
}