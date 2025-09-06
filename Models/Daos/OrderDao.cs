using DBManager;
using DBManager.Framework;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Models.Daos
{
    /// <summary>
    /// 受注ビュー (v_order) にアクセスする DAO クラス。<br/>
    /// 受注ヘッダ情報および受注明細情報をまとめて取得する機能を提供する。
    /// </summary>
    public class OrderDao : BaseDtoDao<OrderDto>
    {
        /// <summary>
        /// 主キーを指定して単一の受注データを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <param name="pkeys">検索キー。</param>
        /// <returns>受注データ。存在しない場合は <c>null</c>。</returns>
        protected override OrderDto? Fetch(params object[] pkeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// すべての受注データを取得する。<br/>
        /// 現在は未実装。
        /// </summary>
        /// <returns>受注データのリスト。</returns>
        protected override List<OrderDto> Find()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定された受注コードに対応する受注データを取得する。<br/>
        /// <paramref name="pkeys"/> の 0 番目に受注コード (orderCd) を渡すことで、<br/>
        /// 受注ヘッダ情報および明細情報をまとめて取得する。
        /// </summary>
        /// <param name="pkeys">
        /// 検索キー配列。  
        /// [0] = orderCd (string)
        /// </param>
        /// <returns>
        /// 受注データ (<see cref="OrderDto"/>) のリスト。<br/>
        /// 1件の受注に対して明細行数分の DTO が生成される。<br/>
        /// 対象の受注が存在しない場合は空のリストを返す。
        /// </returns>
        protected override List<OrderDto> Find(params object[] pkeys)
        {
            var orderCd = pkeys[0].ToString() ?? throw new ArgumentNullException(nameof(pkeys), "[0] = orderCd (string)");

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
                    List<OrderDto> orderDtoList = [];
                    while (reader.Read())
                    {
                        var orderDto = new OrderDto
                        {
                            OrderCd = reader.GetNonNullString("order_cd"),
                            DestinationNo = reader.GetNonNullInt("destination_no"),
                            CustomerId = reader.GetNonNullString("customer_id"),
                            DestinationName = reader.GetNonNullString("destination_name"),
                            Postcode = reader.GetNonNullString("postcode"),
                            Address = reader.GetNonNullString("address"),
                            Phone = reader.GetNonNullString("phone"),
                            SalesCd = reader.GetNonNullString("sales_cd"),
                            ScheduledDeliveryAt = reader.GetNullableDateTime("scheduled_delivery_at"),
                            ShippedAt = reader.GetNullableDateTime("shipped_at"),
                            CancelledAt = reader.GetNullableDateTime("cancelled_at"),
                            JanCd = reader.GetNonNullString("jan_cd"),
                            ItemNm = reader.GetNonNullString("item_nm"),
                            FileNm = reader.GetNonNullString("file_nm"),
                            Price = reader.GetNonNullInt("price"),
                            Qty = reader.GetNonNullInt("qty"),
                        };
                        orderDtoList.Add(orderDto);
                    }
                    return orderDtoList;
                }
            }
        }
    }
}
