using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Models.Services
{
    /// <summary>
    /// 商品に関するユースケースを提供するサービスクラス。
    /// </summary>
    /// <remarks>
    /// 本クラスは商品の在庫・販売情報の一覧取得処理を担当する。
    /// 処理そのものは Action クラスに委譲し、トランザクション管理は <see cref="DatabaseFramework"/> に一任する。
    ///
    /// 【責務】
    /// ・ユースケース単位での処理の組み立て
    /// ・Action の生成および実行
    ///
    /// 【前提】
    /// ・各DAOはDIコンテナにより注入されていること
    /// </remarks>
    public class ItemService(DatabaseFramework dbFramework, ItemSalesStockDao dao)
    {
        /// <summary>
        /// 商品の在庫・販売情報の一覧を取得する。
        /// </summary>
        /// <returns>商品の在庫・販売情報の一覧。該当データが存在しない場合は <c>null</c></returns>
        /// <remarks>
        /// 本メソッドは以下の処理を行う。
        /// <list type="number">
        /// <item><see cref="ItemSalesStockListReadAction"/> を生成</item>
        /// <item><see cref="DatabaseFramework"/> を介して実行</item>
        /// </list>
        ///
        /// 【用途】
        /// ・商品一覧画面表示
        ///
        /// 【設計方針】
        /// ・データ未存在は例外ではなく null で表現する
        /// </remarks>
        public List<ItemSalesStockDto>? GetItemSalesStockList()
        {
            return dbFramework.Execute(new ItemSalesStockListReadAction(dao));
        }
    }
}