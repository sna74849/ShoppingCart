using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Models.Daos
{
    public class OrderDao : BaseDtoDao<OrderDto>
    {
        protected override OrderDto? Fetch(params object[] pkeys)
        {
            throw new NotImplementedException();
        }
        protected override List<OrderDto> Find()
        {
            throw new NotImplementedException();
        }

        protected override List<OrderDto> Find(params object[] pkeys)
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
                    List<OrderDto> orderDtoList = [];
                    while (reader.Read())
                    {
                        var orderDto = new OrderDto
                        {
                            OrderCd = reader.GetNonNullString("order_cd"),
                            DestinationNo = reader.GetNonNullInt("destination_no"),
                            CustomerId = reader.GetNonNullString("customer_id"),
                            DestinationName = reader.GetNonNullString("destination_name"),
                            Postcode = reader.GetNonNullString("postcode"),
                            Address = reader.GetNonNullString("address"),
                            Phone = reader.GetNonNullString("phone"),
                            SalesCd = reader.GetNonNullString("sales_cd"),
                            ScheduledDeliveryAt = reader.GetNullableDateTime("scheduled_delivery_at"),
                            ShippedAt = reader.GetNullableDateTime("shipped_at"),
                            CancelledAt = reader.GetNullableDateTime("cancelled_at"),
                            JanCd = reader.GetNonNullString("jan_cd"),
                            ItemNm = reader.GetNonNullString("item_nm"),
                            FileNm = reader.GetNonNullString("file_nm"),
                            Price = reader.GetNonNullInt("price"),
                            Qty = reader.GetNonNullInt("qty"),
                        };
                        orderDtoList.Add(orderDto);
                    }
                    return orderDtoList;
                }
            }
        }
    }
}

