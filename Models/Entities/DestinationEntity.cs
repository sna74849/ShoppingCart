namespace ShoppingCart.Models.Entities
{
    /// <summary>
    /// 配送先マスタを表すエンティティクラス。
    /// <para>
    /// データベーステーブル: <c>m_destination</c>
    /// </para>
    /// </summary>
    public class DestinationEntity
    {
        /// <summary>
        /// 配送先番号（自動採番、主キー）。
        /// </summary>
        public int DestinationNo { get; set; } = default!;

        /// <summary>
        /// 顧客ID。
        /// <para>
        /// 外部キー: <c>m_customer.customer_id</c>
        /// </para>
        /// </summary>
        public string CustomerId { get; set; } = default!;

        /// <summary>
        /// 配送先名。
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// 郵便番号。
        /// </summary>
        public string Postcode { get; set; } = default!;

        /// <summary>
        /// 住所。
        /// </summary>
        public string Address { get; set; } = default!;

        /// <summary>
        /// 電話番号。
        /// </summary>
        public string Phone { get; set; } = default!;

        /// <summary>
        /// 登録日時（デフォルトは <c>GETDATE()</c>）。
        /// </summary>
        public DateTime CreatedAt { get; set; } = default!;

        /// <summary>
        /// 更新日時（デフォルトは <c>GETDATE()</c>）。
        /// </summary>
        public DateTime UpdatedAt { get; set; } = default!;
    }
}
