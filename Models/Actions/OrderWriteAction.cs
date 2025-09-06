

using DBManager.Framework;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.Exceptions;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Models.Actions
{
    public class OrderWriteAction(int destinationNo, List<CartItemViewModel> cartItemDtoList, List<OrderScheduledDeliveryViewModel> OrderScheduledDeliveryVm) : IServiceAction<string>
    {
        public string? Execute()
        {
            IWritableDao<OrderHeaderEntity> orderHeaderDao = new OrderHeaderDao();
            IWritableDao<OrderDetailEntity> orderDetailDao = new OrderDetailDao();
            IReadableDao<StockEntity> stockReadDao = new StockDao();
            IWritableDao<StockEntity> stockWriteDao = new StockDao();
            IReadableDao<SalesEntity> salesDao = new SalesDao();

            // 注文番号を生成
            var dateTime = DateTime.Now;
            var orderCd = dateTime.Month.ToString() + dateTime.Day.ToString() + dateTime.Hour.ToString() + dateTime.Minute.ToString() + dateTime.Second.ToString();
            orderCd = orderCd.PadLeft(11, '0');

            orderHeaderDao!.Insert(new OrderHeaderEntity
            {
                OrderCd = orderCd,
                DestinationNo = destinationNo
            });
            int itemCnt = 0;
            foreach (CartItemViewModel cartItemDto in cartItemDtoList!)
            {
                var stockEtyList = stockReadDao!.Find(cartItemDto.Item.JanCd, cartItemDto.InCartQty);
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
                    orderDetailDao!.Insert(new OrderDetailEntity
                    {
                        OrderCd = orderCd,
                        SalesCd = salesDao!.Fetch(cartItemDto.Item.JanCd)!.SalesCd,
                        SeqNo = seqNo++,
                        ScheduledDeliveryAt
                            = OrderScheduledDeliveryVm![itemCnt].ScheduledDeliveryIs ? OrderScheduledDeliveryVm[itemCnt].ScheduledDeliveryAt : null,
                        ShippedAt = null
                    });
                    stockEty.OrderCd = orderCd;
                    stockWriteDao!.Patch(stockEty.OrderCd,stockEty.StockNo);
                }
                itemCnt += 1;
            }
            return orderCd;
        }
    }
}
