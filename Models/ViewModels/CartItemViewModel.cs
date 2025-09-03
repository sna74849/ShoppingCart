using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Models.ViewModels
{
    /// <summary>
    /// カートに入っている商品と在庫情報をまとめたViewModel。
    /// </summary>
    public class CartItemViewModel
    {

        public ItemSalesStockDto Item { get; set; } = default!;
        /// <summary>
        /// カートに入っている数量
        /// </summary>
        public int InCartQty { get; set; } = default!;
    }
}
