namespace ShoppingCart.Models.Entities
{
    /// <summary>
    /// 注文ヘッダー情報を表すエンティティクラス。  
    /// 注文ごとの基本的な情報（注文コード、配送先番号、作成日時）を保持します。
    /// <para>
    /// データベーステーブル: <c>t_order_header</c>
    /// </para>
    /// </summary>
    public class OrderHeaderEntity
    {
        /// <summary>
        /// 注文コード。システム内で一意の注文を識別するためのコードです。
        /// </summary>
        public string OrderCd { get; set; } = default!;

        /// <summary>
        /// 配送先番号。ユーザーごとに登録された配送先の識別番号です。
        /// </summary>
        public int DestinationNo { get; set; } = default!;

        /// <summary>
        /// 注文の作成日時。注文データが登録された日時を表します。
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}