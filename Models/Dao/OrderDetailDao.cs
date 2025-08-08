using ShoppingCart.Models.Entity;

namespace ShoppingCart.Models.Dao
{
    public class OrderDetailDao : IBaseEntityDao<OrderDetailEntity>
    {
        public OrderDetailEntity Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        public List<OrderDetailEntity> Find()
        {
            throw new NotImplementedException();
        }

        public List<OrderDetailEntity> FindBy(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        public int Insert(OrderDetailEntity entity)
        {
            string query = @"
                            INSERT INTO t_order_detail 
                            (
                                order_cd, 
                                sales_cd, 
                                seq_no, 
                                scheduled_delivery_at
                            ) 
                            VALUES 
                            (          
                                @orderCd,
                                @salesCd,
                                @seqNo, 
                                @scheduledDeliveryAt
                            )";

            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@orderCd", entity.OrderCd)
                .AddParameter("@salesCd", entity.SalesCd)
                .AddParameter("@seqNo", entity.SeqNo)
                .AddParameter("@scheduledDeliveryAt", entity.ScheduledDeliveryAt)
                .Build();
            {
                return cmd.ExecuteNonQuery();
            }
        }
        public int Update(OrderDetailEntity entity)
        {
            throw new NotImplementedException();
        }
        public int Delete(OrderDetailEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
