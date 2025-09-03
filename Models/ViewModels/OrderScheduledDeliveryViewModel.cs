namespace ShoppingCart.Models.ViewModels
{
    /// <summary>
    /// 商品ごとの配送希望日を指定するためのビューモデル（DTO）。
    /// </summary>
    public class OrderScheduledDeliveryViewModel
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
