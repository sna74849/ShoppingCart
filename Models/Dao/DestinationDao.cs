using ShoppingCart.Models.Entity;

namespace ShoppingCart.Models.Dao {
    public class DestinationDao : IBaseEntityDao<DestinationEntity> {
        public DestinationEntity Find(params object[] pkeys) 
        {
            throw new NotImplementedException();
        }
        public List<DestinationEntity> Find()
        {
            throw new NotImplementedException();
        }
        public List<DestinationEntity> FindBy(params object[] pkeys)
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
                            DestinationNo = reader.GetInt("destination_no"),
                            Name = reader.GetString("name"),
                            Postcode = reader.GetString("postcode"),
                            Address = reader.GetString("address"),
                            Phone = reader.GetString("phone"),
                        });
                    }
                    return destinationEntities;
                }
            }
        }

        public int Insert(DestinationEntity entity)
        {
            throw new NotImplementedException();
        }
        public int Update(DestinationEntity entity)
        {
            throw new NotImplementedException();
        }
        public int Delete(DestinationEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
