using DBManager.Framework;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.Exceptions;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Models.Actions
{
    /// <summary>
    /// 注文登録処理（書き込みユースケース）を実行するアクション。
    /// </summary>
    /// <remarks>
    /// カート内の商品情報を元に以下の処理を一括で実行する。
    /// <list type="number">
    /// <item>注文番号の生成</item>
    /// <item>注文ヘッダの登録</item>
    /// <item>在庫の引当（存在・数量チェック）</item>
    /// <item>注文明細の登録</item>
    /// <item>在庫への注文紐付け（引当確定）</item>
    /// </list>
    ///
    /// 【トランザクション】
    /// 本処理は複数テーブル（注文ヘッダ・明細・在庫）を更新するため、
    /// 呼び出し元（DatabaseService）により単一トランザクション内で実行されることを前提とする。
    ///
    /// 【前提条件】
    /// ・<paramref name="cartItemVmList"/> は null でなく、1件以上の要素を持つこと
    /// ・<paramref name="orderScheduledDeliveryVmList"/> は cartItemVmList と同一順序・同一件数であること
    ///
    /// 【注意】
    /// ・在庫取得および売上情報取得は商品単位で実行されるため、
    ///   データ件数が多い場合はパフォーマンスに注意すること
    /// </remarks>
    /// <param name="destinationNo">配送先番号</param>
    /// <param name="cartItemVmList">カート内の商品一覧</param>
    /// <param name="orderScheduledDeliveryVmList">配送指定情報一覧（商品と同一順序であること）</param>
    /// <param name="orderHeaderDao">注文ヘッダ登録DAO</param>
    /// <param name="orderDetailDao">注文明細登録DAO</param>
    /// <param name="stockDao">在庫参照・更新DAO</param>
    /// <param name="salesDao">売上情報参照DAO</param>
    public class OrderWriteAction(int destinationNo,
                                    List<CartItemViewModel> cartItemVmList,
                                    List<OrderScheduledDeliveryViewModel> orderScheduledDeliveryVmList,
                                    OrderHeaderDao orderHeaderDao,
                                    OrderDetailDao orderDetailDao,
                                    StockDao stockDao,
                                    SalesDao salesDao) : IWriteAction<string>
    {
        /// <summary>
        /// 注文ヘッダ登録DAO
        /// </summary>
        private readonly IWritableDao<OrderHeaderEntity> _orderHeaderWriteDao = orderHeaderDao;

        /// <summary>
        /// 注文明細登録DAO
        /// </summary>
        private readonly IWritableDao<OrderDetailEntity> _orderDetailWriteDao = orderDetailDao;

        /// <summary>
        /// 在庫参照DAO
        /// </summary>
        private readonly IReadableDao<StockEntity> _stockReadDao = stockDao;

        /// <summary>
        /// 在庫更新DAO
        /// </summary>
        private readonly IWritableDao<StockEntity> _stockWriteDao = stockDao;

        /// <summary>
        /// 売上情報参照DAO
        /// </summary>
        private readonly IReadableDao<SalesEntity> _salesReadDao = salesDao;

        /// <summary>
        /// 注文登録処理を実行する。
        /// </summary>
        /// <returns>生成された注文番号</returns>
        /// <remarks>
        /// 本メソッドは以下の手順で処理を行う。
        /// <list type="number">
        /// <item>現在日時を基に注文番号を生成する</item>
        /// <item>注文ヘッダを登録する</item>
        /// <item>商品ごとに必要な在庫を取得し、数量チェックを行う</item>
        /// <item>在庫1件ごとに注文明細を登録する</item>
        /// <item>該当在庫に注文番号を設定し、引当を確定する</item>
        /// </list>
        ///
        /// 【戻り値】
        /// ・正常時：生成された注文番号
        /// ・異常時：例外をスローする
        ///
        /// 【実装上の注意】
        /// ・売上情報は商品ごとに取得しているため、呼び出し回数に注意
        /// ・配送情報はインデックスで対応付けているため、順序不整合に注意
        /// </remarks>
        /// <exception cref="OrderException">
        /// 在庫が存在しない、または必要数量に満たない場合
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// 売上情報が取得できない場合（Fetch結果が null）
        /// </exception>
        public string? ExecuteNonQuery()
        {
            // 注文番号を生成
            var dateTime = DateTime.Now;
            var orderCd = dateTime.Month.ToString() + dateTime.Day.ToString() + dateTime.Hour.ToString() + dateTime.Minute.ToString() + dateTime.Second.ToString();
            orderCd = orderCd.PadLeft(11, '0');

            _orderHeaderWriteDao.Insert(new OrderHeaderEntity
            {
                OrderCd = orderCd,
                DestinationNo = destinationNo
            });

            int itemCnt = 0;

            foreach (CartItemViewModel cartItemDto in cartItemVmList!)
            {
                var stockEtyList = _stockReadDao.Find(cartItemDto.Item.JanCd, cartItemDto.InCartQty);

                if (stockEtyList.Count < cartItemDto.InCartQty)
                {
                    throw new OrderException("在庫が不足しています。");
                }
                else if (stockEtyList.Count == 0)
                {
                    throw new OrderException("在庫がありません。");
                }

                int seqNo = 1;

                foreach (var stockEty in stockEtyList)
                {
                    _orderDetailWriteDao.Insert(new OrderDetailEntity
                    {
                        OrderCd = orderCd,
                        SalesCd = _salesReadDao.Fetch(cartItemDto.Item.JanCd)!.SalesCd,
                        SeqNo = seqNo++,
                        ScheduledDeliveryAt
                            = orderScheduledDeliveryVmList[itemCnt].ScheduledDeliveryIs
                                ? orderScheduledDeliveryVmList[itemCnt].ScheduledDeliveryAt
                                : null,
                        ShippedAt = null
                    });

                    stockEty.OrderCd = orderCd;
                    _stockWriteDao.Patch(stockEty.OrderCd, stockEty.StockNo);
                }
                itemCnt += 1;
            }
            return orderCd;
        }
    }
}