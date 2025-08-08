using ShoppingCart.Models.Entity;
using System.Collections.Generic;

namespace ShoppingCart.Models.Dto
{
    public class OrderDto {

        /// <summary>
        /// 注文コード。システム内で一意の注文を識別するためのコードです。
        /// </summary>
        public string OrderCd { get; set; } = default!;
        public DestinationEntity? Destination { get; set; }
        public List<OrderItemDto>? OrderDetails { get; set; }
    }
}
