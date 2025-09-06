using DBManager.Framework;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Models.Actions
{
    public class OrderReadAction(string orderCd) : IServiceAction<OrderViewModel>
    {
        public OrderViewModel? Execute()
        {
            IReadableDao<OrderDto> orderDao = new OrderDao();
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
