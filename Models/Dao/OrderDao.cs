<<<<<<< HEAD
﻿using ShoppingCart.Models.Dto;
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
=======
﻿using DynamicDll.Db;
using ShoppingCart.Models.Entity;
using System.Data.SqlClient;

namespace ShoppingCart.Models.Dao {
    public class OrderDao : BaseDao<OrderEntity> {
        public override int Delete(OrderEntity entity) {
            throw new NotImplementedException();
        }

        public override OrderEntity Find(params object[] pkeys) {
            throw new NotImplementedException();
        }

        public override int Insert(OrderEntity entity) {
            string query = @"INSERT INTO t_order(order_cd, branch_no, customer_id, item_cd, creade_at)";
            query += "VALUES(@orderCd, @branchNo, @customerId, @itemCd, GETDATE())";

            using (SqlCommand cmd = new SqlCommand(query, con)) {
                cmd.Parameters.Add(new SqlParameter("@orderCd", System.Data.SqlDbType.Char)).Value = entity.OrderCd;
                cmd.Parameters.Add(new SqlParameter("@branchNo", System.Data.SqlDbType.Int)).Value = entity.BranchNo;
                cmd.Parameters.Add(new SqlParameter("@customerId", System.Data.SqlDbType.VarChar)).Value = entity.CustomerId;
                cmd.Parameters.Add(new SqlParameter("@itemCd", System.Data.SqlDbType.Char)).Value = entity.ItemCd;
                cmd.Transaction = trn;

                return cmd.ExecuteNonQuery();
            }
        }

        public override int Update(OrderEntity entity) {
>>>>>>> 91decb47f3a5019fbd405ce4ab88d86b6396da4d
            throw new NotImplementedException();
        }
    }
}
<<<<<<< HEAD

=======
>>>>>>> 91decb47f3a5019fbd405ce4ab88d86b6396da4d
