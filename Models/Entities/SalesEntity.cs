namespace ShoppingCart.Models.Entities
{
    /// <summary>
    /// 
    /// 商品情報を表すエンティティクラス。
    /// 
    /// このクラスは、商品コード、JANコード、価格、登録日時、および削除フラグを含む。
    /// <para>
    /// データベーステーブル: <c>m_sales</c>
    /// </para>
    /// </summary>
    public class SalesEntity
    {
        /// <summary>
        /// 商品コード（10桁固定）
        /// </summary>
        public string SalesCd { get; set; } = default!;

        /// <summary>
        /// JANコード（外部キーだが、ここでは単なる値として扱う）
        /// </summary>
        public string JanCd { get; set; } = default!;

        /// <summary>
        /// 価格
        /// </summary>
        public int Price { get; set; } = default!;

        /// <summary>
        /// 登録日時（デフォルト値あり）
        /// </summary>
        public DateTime CreatedAt { get; set; } = default!;

        /// <summary>
        /// 削除フラグ（0=有効, 1=削除）
        /// </summary>
        public byte DelFlag { get; set; } = default!;
    }
}
