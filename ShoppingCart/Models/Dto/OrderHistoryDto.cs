namespace ShoppingCart.Models.Dto
{
    public class OrderHistoryDto {
        public string OrderCd { get; set; }
        public int BranchNo { get; set; }
        public string ItemCd { get; set; }
        public string ItemNm { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
        public int DestinationNo { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Postcode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime DeliveryAt { get; set; }
    }
}
