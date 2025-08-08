namespace ShoppingCart.Models.Dto
{
    /// <summary>
    /// 商品ごとの配送希望日を指定するためのデータ転送オブジェクト（DTO）。
    /// </summary>
    public class OrderScheduledDeliveryDto
    {
        /// <summary>
        /// 商品コード（JANコードなど）を表します。
        /// </summary>
        public string ItemCd { get; set; } = default!;

        /// <summary>
        /// 配送日を指定するかどうかのフラグ。  
        /// チェックボックスに対応し、true なら配送日を指定、false なら指定しない。
        /// </summary>
        public bool ScheduledDeliveryIs { get; set; } = false;

        /// <summary>
        /// 希望する配送日。  
        /// <see cref="ScheduledDeliveryIs"/> が false の場合は null となる可能性があります。
        /// </summary>
        public DateTime? ScheduledDeliveryAt { get; set; } = null!;
    }
}
