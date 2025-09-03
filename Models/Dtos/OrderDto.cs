namespace ShoppingCart.Models.Dtos
{
    /// <summary>
    /// 注文ビュー (v_order) のDTO。
    /// 注文ヘッダー、配送先、商品明細を結合した結果を表します。
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// 注文コード。システム内で一意に注文を識別するコードです。
        /// </summary>
        public string OrderCd { get; set; } = default!;

        /// <summary>
        /// 配送先番号。配送先マスタと紐付けるためのキーです。
        /// </summary>
        public int DestinationNo { get; set; } = 0;

        /// <summary>
        /// 顧客ID。注文者を識別するためのIDです。
        /// </summary>
        public string CustomerId { get; set; } = default!;

        /// <summary>
        /// 配送先名。顧客マスタに登録されている配送先の名称です。
        /// </summary>
        public string DestinationName { get; set; } = default!;

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
        /// 売上コード。販売管理上のコードです。
        /// </summary>
        public string SalesCd { get; set; } = default!;

        /// <summary>
        /// 配送予定日。
        /// </summary>
        public DateTime? ScheduledDeliveryAt { get; set; }

        /// <summary>
        /// 出荷日。
        /// </summary>
        public DateTime? ShippedAt { get; set; } = null;

        /// <summary>
        /// キャンセル日。
        /// </summary>
        public DateTime? CancelledAt { get; set; } = null;

        /// <summary>
        /// JANコード。商品を識別するコードです。
        /// </summary>
        public string JanCd { get; set; } = default!;

        /// <summary>
        /// 商品名。
        /// </summary>
        public string ItemNm { get; set; } = default!;

        /// <summary>
        /// ファイル名（商品画像などに利用）。
        /// </summary>
        public string FileNm { get; set; } = default!;

        /// <summary>
        /// 商品単価。
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 注文数量。
        /// </summary>
        public int Qty { get; set; }
    }
}
