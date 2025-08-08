namespace ShoppingCart.Models.Dto
{
    /// <summary>
    /// カートに入っている商品と在庫情報をまとめたDTOクラス。
    /// </summary>
    public class CartItemDto
    {

        public ItemSalesStockDto item { get; set; } = default!;
        /// <summary>
        /// カートに入っている数量
        /// </summary>
        public int InCartQty { get; set; } = default!;
    }
}
