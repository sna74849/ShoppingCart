using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    public class SalesDao : BaseEntityDao<SalesEntity>
    {
        protected override SalesEntity? Fetch(params object[] pkeys)
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
                        SalesCd = reader.GetNonNullString("sales_cd"),
                        Price = reader.GetNonNullInt("price"),
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        protected override List<SalesEntity> Find()
        {
            throw new NotImplementedException();
        }

        protected override List<SalesEntity> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        protected override int Insert(SalesEntity t)
        {
            throw new NotImplementedException();
        }

        protected override int Update(SalesEntity t)
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
