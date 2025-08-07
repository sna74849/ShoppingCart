namespace ShoppingCart.Models.Entity
{
    /// <summary>
    /// 注文明細情報を表すエンティティクラス。  
    /// 注文に含まれる各商品の詳細情報を保持します。
    /// </summary>
    public class OrderDetailEntity
    {
        /// <summary>
        /// 注文コード。注文ヘッダーと関連付けられる注文識別子です。
        /// </summary>
        public string OrderCd { get; set; } = default!;

        /// <summary>
        /// 販売コード。商品やサービスを識別するコードです。
        /// </summary>
        public string SalesCd { get; set; } = default!;

        /// <summary>
        /// 明細番号。注文内での行番号や商品ごとの識別番号です。
        /// </summary>
        public int SeqNo { get; set; } = default!;

        /// <summary>
        /// 配送予定日。ユーザーが指定した希望配送日を表します。  
        /// 指定されていない場合は null になります。
        /// </summary>
        public DateTime? ScheduledDeliveryAt { get; set; } = null;

        /// <summary>
        /// 出荷日時。商品が実際に出荷された日時を表します。  
        /// 未出荷の場合は null になります。
        /// </summary>
        public DateTime? ShippedAt { get; set; } = null;

        /// <summary>
        /// キャンセル日時。明細がキャンセルされた日時を表します。  
        /// キャンセルされていない場合は null になります。
        /// </summary>
        public DateTime? CancelledAt { get; set; } = null;

        /// <summary>
        /// 作成日時。この明細情報がシステムに登録された日時です。
        /// </summary>
        public DateTime CreatedAt { get; set; } = default!;
    }
}