using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    public class OrderDetailDao : BaseEntityDao<OrderDetailEntity>
    {
        protected override OrderDetailEntity Fetch(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        protected override List<OrderDetailEntity> Find()
        {
            throw new NotImplementedException();
        }

        protected override List<OrderDetailEntity> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        protected override int Insert(OrderDetailEntity entity)
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

        protected override int Update(OrderDetailEntity t)
        {
            throw new NotImplementedException();
        }
        protected override int Delete(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        protected override int Patch(object value, params object[] pkeys)
        {
            throw new NotImplementedException();
        }
    }
}
