using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos {
    public class DestinationDao : BaseEntityDao<DestinationEntity>
    {
        protected override List<DestinationEntity> Find()
        {
            throw new NotImplementedException();
        }
        protected override List<DestinationEntity> Find(params object[] pkeys)
        {
            string query = @"
                            SELECT 
                                destination_no, 
                                name, 
                                postcode, 
                                address, 
                                phone 
                            FROM 
                                m_destination
                            WHERE 
                                customer_id = @customerId";

            using var com = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@customerId", pkeys[0])
                .Build();
            {
                using var reader = com.ExecuteReader();
                {
                    List<DestinationEntity> destinationEntities = new();

                    while (reader.Read())
                    {
                        destinationEntities.Add(new DestinationEntity
                        {
                            DestinationNo = reader.GetNonNullInt("destination_no"),
                            Name = reader.GetNonNullString("name"),
                            Postcode = reader.GetNonNullString("postcode"),
                            Address = reader.GetNonNullString("address"),
                            Phone = reader.GetNonNullString("phone"),
                        });
                    }
                    return destinationEntities;
                }
            }
        }

        protected override int Insert(DestinationEntity t)
        {
            throw new NotImplementedException();
        }
        protected override int Update(DestinationEntity t)
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

        protected override DestinationEntity? Fetch(params object[] pkeys)
        {
            throw new NotImplementedException();
        }
    }
}
