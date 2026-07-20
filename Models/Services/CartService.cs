using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Exceptions;

namespace ShoppingCart.Models.Services
{
    /// <summary>
    /// カートに関するユースケースを提供するサービスクラス。
    /// </summary>
    /// <remarks>
    /// 本クラスは商品の在庫・販売情報の取得処理を担当する。
    /// 処理そのものは Action クラスに委譲し、トランザクション管理は <see cref="DatabaseFramework"/> に一任する。
    ///
    /// 【責務】
    /// ・ユースケース単位での処理の組み立て
    /// ・Action の生成および実行
    ///
    /// 【前提】
    /// ・各DAOはDIコンテナにより注入されていること
    /// </remarks>
    public class CartService(DatabaseFramework dbFramework, ItemSalesStockDao dao)
    {
        /// <summary>
        /// JANコードを指定して商品の在庫・販売情報を取得する。
        /// </summary>
        /// <param name="janCd">JANコード（null・空文字不可）</param>
        /// <returns>商品の在庫・販売情報</returns>
        /// <remarks>
        /// 本メソッドは以下の処理を行う。
        /// <list type="number">
        /// <item><see cref="ItemSalesStockReadAction"/> を生成</item>
        /// <item><see cref="DatabaseFramework"/> を介して実行</item>
        /// </list>
        ///
        /// 【用途】
        /// ・カートへの商品追加時の在庫確認
        ///
        /// 【設計方針】
        /// ・データ未存在（在庫なし）は <see cref="StockException"/> をスローして表現する
        /// </remarks>
        /// <exception cref="StockException">
        /// 指定されたJANコードに該当する在庫情報が存在しない場合
        /// </exception>
        public ItemSalesStockDto GetItem(string janCd)
        {
            return dbFramework.Execute(new ItemSalesStockReadAction(janCd, dao)) ?? throw new StockException("在庫情報がありません。");
        }
    }
}