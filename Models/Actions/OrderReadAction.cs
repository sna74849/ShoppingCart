using DBManager.Framework;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Models.Actions
{
    /// <summary>
    /// 注文情報取得（読み取りユースケース）を実行するアクション。
    /// </summary>
    /// <remarks>
    /// 指定された注文番号を元に注文情報および注文明細を取得し、
    /// <see cref="OrderViewModel"/> に変換して返却する。
    ///
    /// 【処理概要】
    /// ・DAOから注文情報（ヘッダ＋明細の結合データ）を取得
    /// ・配送先情報を生成（先頭レコードを基準とする）
    /// ・注文明細情報をリストとして構築
    /// ・ViewModelへマッピング
    ///
    /// 【前提条件】
    /// ・<paramref name="orderCd"/> は null または空文字でないこと
    ///
    /// 【設計方針】
    /// ・該当データが存在しない場合は例外ではなく null を返却する
    /// ・DTOはDB構造に依存し、ViewModelは画面表示用に整形される
    /// </remarks>
    /// <param name="orderCd">取得対象の注文番号</param>
    /// <param name="orderDao">注文情報取得用DAO（注文ヘッダ・明細の結合データを返却する）</param>
    public class OrderReadAction(string orderCd, OrderDao orderDao) : IReadAction<OrderViewModel>
    {
        /// <summary>
        /// 注文情報取得用のDAO
        /// </summary>
        private readonly IReadableDao<OrderDto> _orderDao = orderDao;

        /// <summary>
        /// 注文情報を取得する。
        /// </summary>
        /// <returns>
        /// 注文情報を格納した <see cref="OrderViewModel"/>。
        /// 対象の注文が存在しない場合は <c>null</c> を返す。
        /// </returns>
        /// <remarks>
        /// 本メソッドは以下の手順で処理を行う。
        /// <list type="number">
        /// <item>注文番号をキーにDAOからデータを取得する</item>
        /// <item>取得結果が存在しない場合は null を返却する</item>
        /// <item>先頭レコードを基に配送先情報を構築する</item>
        /// <item>全レコードを走査し、注文明細リストを生成する</item>
        /// <item><see cref="OrderViewModel"/> にマッピングして返却する</item>
        /// </list>
        ///
        /// 【注意】
        /// ・配送先情報は取得結果の先頭要素を基に生成しているため、
        ///   DAOは同一注文に対して一貫したデータを返すことを前提とする
        ///
        /// 【パフォーマンス】
        /// ・本処理は単一クエリ結果をメモリ上で整形するのみであり、
        ///   追加のDBアクセスは発生しない
        /// </remarks>
        public OrderViewModel? ExecuteQuery()
        {
            var orderDtoList = _orderDao.Find(orderCd);

            if (orderDtoList.Count > 0)
            {
                var orderViewModel = new OrderViewModel
                {
                    OrderCd = orderCd,
                    Destination = new DestinationEntity
                    {
                        DestinationNo = orderDtoList[0].DestinationNo,
                        CustomerId = orderDtoList[0].CustomerId,
                        Name = orderDtoList[0].DestinationName,
                        Postcode = orderDtoList[0].Postcode,
                        Address = orderDtoList[0].Address,
                        Phone = orderDtoList[0].Phone,
                    }
                };

                List<OrderItemDto> orderItemDtoList = [];

                foreach (var dto in orderDtoList)
                {
                    orderItemDtoList.Add(new OrderItemDto
                    {
                        SalesCd = dto.SalesCd,
                        ScheduledDeliveryAt = dto.ScheduledDeliveryAt,
                        ShippedAt = dto.ShippedAt,
                        CancelledAt = dto.CancelledAt,
                        Item = new ItemSalesStockDto
                        {
                            JanCd = dto.JanCd,
                            ItemNm = dto.ItemNm,
                            FileNm = dto.FileNm,
                            Price = dto.Price,
                            Qty = dto.Qty,
                        }
                    });
                }

                orderViewModel.OrderDetails = orderItemDtoList;
                return orderViewModel;
            }
            else
            {
                return null;
            }
        }
    }
}