using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Daos
{
    /// <summary>
    /// 顧客情報テーブル (m_customer) にアクセスするための DAO クラス。
    /// 認証処理や顧客データの取得・更新を担当する。
    /// </summary>
    public class CustomerDao : BaseEntityDao<CustomerEntity>
    {
        /// <summary>
        /// 主キー（または検索キー）を指定して顧客情報を取得する。
        /// 本実装では <paramref name="pkeys"/> の 0 番目にメールアドレス、1 番目にパスワードを渡すことで、
        /// m_customer テーブルから該当する顧客を検索する。
        /// </summary>
        /// <param name="pkeys">
        /// 検索キー配列。  
        /// [0] = email (string)  
        /// [1] = password (string)
        /// </param>
        /// <returns>
        /// 顧客情報 (<see cref="CustomerEntity"/>) を返す。  
        /// 見つからなかった場合は <c>null</c> を返す。
        /// </returns>
        protected override CustomerEntity? Fetch(params object[] pkeys)
        {
            var email = pkeys[0].ToString() ?? throw new ArgumentNullException(nameof(pkeys), "[0] = email (string)");
            var password = pkeys[1].ToString() ?? throw new ArgumentNullException(nameof(pkeys), "[1] = password (string)");
            string query = @"
                            SELECT 
                                customer_id,
                                email 
                            FROM 
                                m_customer 
                            WHERE 
                                email = @email 
                            AND 
                                password = @password";

            using var com = new SqlCommandBuilder()
                .WithCommandText(query)
                .AddParameter("@email", email)
                .AddParameter("@password", password)
                .Build();
            {
                using var reader = com.ExecuteReader();
                {
                    if (reader.Read())
                    {
                        return new CustomerEntity
                        {
                            CustomerId = reader.GetNonNullString("customer_id"),
                            Email = reader.GetNonNullString("email")!,
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 顧客情報をすべて取得する。
        /// 未実装。
        /// </summary>
        protected override List<CustomerEntity> Find()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して顧客情報を取得する。
        /// 未実装。
        /// </summary>
        protected override List<CustomerEntity> Find(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新規に顧客情報を追加する。
        /// 未実装。
        /// </summary>
        protected override int Insert(CustomerEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 顧客情報を更新する。
        /// 未実装。
        /// </summary>
        protected override int Update(CustomerEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 主キーを指定して顧客情報を削除する。
        /// 未実装。
        /// </summary>
        protected override int Delete(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 顧客情報の一部を更新する。
        /// 未実装。
        /// </summary>
        protected override int Patch(object value, params object[] pkeys)
        {
            throw new NotImplementedException();
        }
    }
}
