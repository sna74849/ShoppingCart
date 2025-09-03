namespace ShoppingCart.Models.Entities
{
    /// <summary>
    /// 在庫テーブルを表すエンティティクラス。
    /// <para>
    /// データベーステーブル: <c>t_stock</c>
    /// </para>
    /// </summary>
    public class StockEntity
    {
        /// <summary>
        /// 在庫番号（自動採番、主キー）。
        /// </summary>
        public int StockNo { get; set; } = default!;

        /// <summary>
        /// JANコード（商品を一意に識別するコード）。
        /// <para>
        /// 外部キー: <c>m_item.jan_cd</c>
        /// </para>
        /// </summary>
        public string JanCd { get; set; } = default!;

        /// <summary>
        /// 注文コード。注文に紐付く場合に設定される。
        /// </summary>
        public string? OrderCd { get; set; } = null;

        /// <summary>
        /// 枝番。レコードの一意性を補足する番号。
        /// </summary>
        public int BranchNo { get; set; } = default!;

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
