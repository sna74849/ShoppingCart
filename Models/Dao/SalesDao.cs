using ShoppingCart.Models.Entity;

namespace ShoppingCart.Models.Dao
{
    public class SalesDao : IBaseDtoDao<SalesEntity>
    {
        public SalesEntity? Find(params object[] pkeys)
        {
            string query = @"
                            SELECT 
                                sales_cd, 
                                price
                            FROM 
                                v_sales
                            WHERE 
                                jan_cd = @JanCd";
            using var cmd = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@JanCd", pkeys[0])
                .Build();
            {
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new SalesEntity
                    {
                        SalesCd = reader.GetString("sales_cd"),
                        Price = reader.GetInt("price"),
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public List<SalesEntity> Find()
        {
            throw new NotImplementedException();
        }

        public List<SalesEntity> FindBy(params object[] pkeys)
        {
            throw new NotImplementedException();
        }
    }
}
