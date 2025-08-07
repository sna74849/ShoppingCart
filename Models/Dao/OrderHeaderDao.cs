using ShoppingCart.Models.Entity;

namespace ShoppingCart.Models.Dao
{
    public class OrderHeaderDao : IBaseEntityDao<OrderHeaderEntity>
    {
        public OrderHeaderEntity Find(params object[] pkeys) 
        {
            throw new NotImplementedException();
        }

        public List<OrderHeaderEntity> Find()
        {
            throw new NotImplementedException();
        }

        public List<OrderHeaderEntity> FindBy(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        public int Insert(OrderHeaderEntity entity)
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
        public int Update(OrderHeaderEntity entity)
        {
            throw new NotImplementedException();
        }
        public int Delete(OrderHeaderEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
