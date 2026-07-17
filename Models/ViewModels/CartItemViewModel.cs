using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Models.ViewModels
{
    /// <summary>
    /// カートに入っている商品と在庫情報をまとめたViewModel。
    /// </summary>
    public class CartItemViewModel
    {
        /// <summary>
        /// JANコード
        /// </summary>
        public string JanCd { get; set; } = default!;

        /// <summary>
        /// カートに入っている数量
        /// </summary>
        public int Qty { get; set; } = default!;
    }
}
