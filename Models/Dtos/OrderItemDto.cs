using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Dtos
{
    /// <summary>
    /// 注文内の商品の情報を表し、商品コード、配送予定日、数量などの詳細を含みます。
    /// </summary>
    /// <remarks>
    /// このデータ転送オブジェクト（DTO）は、個々の注文商品の情報をカプセル化するために使用されます。
    /// 注文コード、商品コード、配送予定日、出荷日、数量などの詳細を含みます。
    /// </remarks>
    public class OrderItemDto : OrderDetailEntity
    {
        public ItemSalesStockDto Item { get; set; } = default!;
    }
}
