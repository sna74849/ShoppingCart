namespace ShoppingCart.Models.Entity {
    public class OrderEntity {
        public string OrderCd { get; set; }
        public int BranchNo { get; set; }
        public string CustomerId { get; set; }
        public string ItemCd { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
