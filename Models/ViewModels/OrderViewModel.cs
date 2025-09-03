using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.ViewModels
{
    /// <summary>
    /// 注文情報をまとめたViewModel。
    /// </summary>
    public class OrderViewModel {

        /// <summary>
        /// 注文コード。システム内で一意の注文を識別するためのコードです。
        /// </summary>
        public string OrderCd { get; set; } = default!;
        public DestinationEntity? Destination { get; set; }
        public List<OrderItemDto>? OrderDetails { get; set; }
    }
}
