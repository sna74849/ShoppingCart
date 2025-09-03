using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    public class OrderHeaderDao : BaseEntityDao<OrderHeaderEntity>
    {
        protected override OrderHeaderEntity Fetch(params object[] pkeys) 
        {
            throw new NotImplementedException();
        }

        protected override List<OrderHeaderEntity> Find()
        {
            throw new NotImplementedException();
        }

        protected override List<OrderHeaderEntity> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        protected override int Insert(OrderHeaderEntity entity)
        {
            string query = @"
                            INSERT INTO 
                                t_order_header
                                (
                                    order_cd,
                                    destination_no
                                )
                            VALUES
                                (   
                                    @orderCd,   
                                    @destinationNo
                                )";
            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@orderCd", entity.OrderCd)
                .AddParameter("@destinationNo", entity.DestinationNo)
                .Build();
            {
                return cmd.ExecuteNonQuery();
            }
        }
        protected override int Update(OrderHeaderEntity entity)
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
