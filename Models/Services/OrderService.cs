using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Models.Services
{
    /// <summary>
    /// 注文に関するユースケースを提供するサービスクラス。
    /// </summary>
    /// <remarks>
    /// 本クラスは注文の「登録（書き込み）」および「参照（読み取り）」を担当する。
    /// 各処理は Action クラスに委譲し、トランザクション管理は <see cref="DatabaseFramework"/> に一任する。
    ///
    /// 【責務】
    /// ・ユースケース単位での処理の組み立て
    /// ・Action の生成および実行
    /// ・トランザクション境界の統一
    ///
    /// 【前提】
    /// ・各DAOはDIコンテナにより注入されていること
    /// </remarks>
    public class OrderService(DatabaseFramework dbFramework,
                            OrderDao orderDao,
                            OrderHeaderDao orderHeaderDao,
                            OrderDetailDao orderDetailDao,
                            StockDao stockDao,
                            SalesDao salesDao)
    {
        /// <summary>
        /// 注文を新規登録する。
        /// </summary>
        /// <param name="destinationNo">配送先番号</param>
        /// <param name="cartItemVmList">カート内の商品一覧（null不可・1件以上）</param>
        /// <param name="deliveryList">配送予定情報一覧（商品と同一順序・同一件数）</param>
        /// <returns>登録された注文コード</returns>
        /// <remarks>
        /// 本メソッドは以下の処理を行う。
        /// <list type="number">
        /// <item><see cref="OrderWriteAction"/> を生成</item>
        /// <item><see cref="DatabaseFramework"/> を介して実行（トランザクション管理含む）</item>
        /// </list>
        ///
        /// 【トランザクション】
        /// 注文登録は複数テーブル更新を伴うため、
        /// DatabaseService により単一トランザクションとして実行される。
        ///
        /// 【前提条件】
        /// ・cartItemVmList は null でなく、空でないこと
        /// ・deliveryList は cartItemVmList と対応する順序・件数であること
        ///
        /// 【戻り値】
        /// ・正常時：注文コード
        /// ・異常時：例外をスローする（nullは返さない想定）
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// 引数が不正な場合（商品リストが空、配送情報と不整合など）
        /// </exception>
        /// <exception cref="Exception">
        /// 注文登録処理中にエラーが発生した場合（DBエラー含む）
        /// </exception>
        public string CreateOrder(int destinationNo,
                             List<CartItemViewModel> cartItemVmList,
                             List<OrderScheduledDeliveryViewModel> deliveryList)
        {
            var action = new OrderWriteAction(
                destinationNo,
                cartItemVmList,
                deliveryList,
                orderHeaderDao,
                orderDetailDao,
                stockDao,
                salesDao);

            return dbFramework.Execute(action)!;
        }

        /// <summary>
        /// 注文コードを指定して注文情報を取得する。
        /// </summary>
        /// <param name="orderCd">注文コード（null・空文字不可）</param>
        /// <returns>注文情報。該当データが存在しない場合は <c>null</c></returns>
        /// <remarks>
        /// 本メソッドは以下の処理を行う。
        /// <list type="number">
        /// <item><see cref="OrderReadAction"/> を生成</item>
        /// <item><see cref="DatabaseFramework"/> を介して実行</item>
        /// </list>
        ///
        /// 【用途】
        /// ・注文詳細画面表示
        /// ・注文確認処理
        ///
        /// 【設計方針】
        /// ・データ未存在は例外ではなく null で表現する
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// orderCd が null または空文字の場合
        /// </exception>
        public OrderViewModel? GetOrder(string orderCd)
        {
            if (string.IsNullOrWhiteSpace(orderCd))
            {
                throw new ArgumentNullException(nameof(orderCd));
            }

            return dbFramework.Execute(new OrderReadAction(orderCd, orderDao));
        }
    }
}