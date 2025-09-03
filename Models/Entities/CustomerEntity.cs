namespace ShoppingCart.Models.Entities
{
    /// <summary>
    /// 顧客マスタを表すエンティティクラス。
    /// <para>
    /// データベーステーブル: <c>m_customer</c>
    /// </para>
    /// </summary>
    public class CustomerEntity
    {
        /// <summary>
        /// 顧客ID（主キー）。
        /// </summary>
        public string CustomerId { get; set; } = default!;

        /// <summary>
        /// パスワード。
        /// <para>
        /// データベース上では <c>VARCHAR(40)</c> で保存される。
        /// </para>
        /// </summary>
        public string Password { get; set; } = default!;

        /// <summary>
        /// メールアドレス（ユニーク制約あり）。
        /// </summary>
        public string Email { get; set; } = default!;

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
