using ShoppingCart.Models.Dto;
using ShoppingCart.Models.Entity;

namespace ShoppingCart.Models.Dao
{
    public class OrderDao : IBaseDtoDao<OrderDto>
    {
        public OrderDto? Find(params object[] pkeys)
        {
            var orderCd = (string)pkeys[0];
            string query = @"
                            SELECT 
                                order_cd,
                                destination_no,
                                customer_id,
                                destination_name,
                                postcode,
                                address,
                                phone,
                                sales_cd,
                                scheduled_delivery_at,
                                shipped_at,
                                cancelled_at,
                                jan_cd,
                                item_nm,
                                file_nm,
                                price,
                                qty
                            FROM 
                                v_order o
                            WHERE 
                                order_cd = @orderCd
                            ";
            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("orderCd", orderCd)
                .Build();
            {
                using var reader = cmd.ExecuteReader();
                {
                    List <OrderItemDto> OderItemDtoList = new();
                    if (reader.Read())
                    {
                        var orderDto =  new OrderDto
                        {
                            OrderCd = reader.GetString("order_cd"),
                            Destination = new DestinationEntity
                            {
                                DestinationNo = reader.GetInt("destination_no"),
                                CustomerId = reader.GetString("customer_id"),
                                Name = reader.GetString("destination_name"),
                                Postcode = reader.GetString("postcode"),
                                Address = reader.GetString("address"),
                                Phone = reader.GetString("phone"),
                            }
                        };
                        do
                        {
                           OderItemDtoList.Add(new OrderItemDto
                            {
                                SalesCd = reader.GetString("sales_cd"),
                                ScheduledDeliveryAt = reader.GetNullableDateTime("scheduled_delivery_at"),
                                ShippedAt = reader.GetNullableDateTime("shipped_at"),
                                CancelledAt = reader.GetNullableDateTime("cancelled_at"),
                                Item = new ItemSalesStockDto
                                {
                                    JanCd = reader.GetString("jan_cd"),
                                    ItemNm = reader.GetString("item_nm"),
                                    FileNm = reader.GetString("file_nm"),
                                    Price = reader.GetInt("price"),
                                    Qty = reader.GetInt("qty"),
                                }
                            });
                        } while (reader.Read());
                        orderDto.OrderDetails = OderItemDtoList;
                        return orderDto;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public List<OrderDto> Find()
        {
            throw new NotImplementedException();
        }

        public List<OrderDto> FindBy(params object[] pkeys)
        {
            throw new NotImplementedException();
        }
    }
}

