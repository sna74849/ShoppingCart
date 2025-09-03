

using DBManager.Framework;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.Exceptions;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Models.Actions
{
    public class OrderWriteAction(int destinationNo, List<CartItemViewModel> cartItemDtoList, List<OrderScheduledDeliveryViewModel> OrderScheduledDeliveryVm) : IWriteAction<OrderViewModel>
    {
        public OrderViewModel? Execute()
        {
            IWritableDao<OrderHeaderEntity> orderHeaderDao = new OrderHeaderDao();
            IWritableDao<OrderDetailEntity> orderDetailDao = new OrderDetailDao();
            IReadableDao<StockEntity> stockReadDao = new StockDao();
            IWritableDao<StockEntity> stockWriteDao = new StockDao();
            IReadableDao<SalesEntity> salesDao = new SalesDao();
            IReadableDao<OrderDto> orderDao = new OrderDao();

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
            var orderDtoList = orderDao!.Find(orderCd);
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
                var loop = 0;
                do
                {
                    orderItemDtoList.Add(new OrderItemDto
                    {
                        SalesCd = orderDtoList[loop].SalesCd,
                        ScheduledDeliveryAt = orderDtoList[loop].ScheduledDeliveryAt,
                        ShippedAt = orderDtoList[loop].ShippedAt,
                        CancelledAt = orderDtoList[loop].CancelledAt,
                        Item = new ItemSalesStockDto
                        {
                            JanCd = orderDtoList[loop].JanCd,
                            ItemNm = orderDtoList[loop].ItemNm,
                            FileNm = orderDtoList[loop].FileNm,
                            Price = orderDtoList[loop].Price,
                            Qty = orderDtoList[loop].Qty,
                        }
                    });
                } while (orderDtoList.Count > ++loop);
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
