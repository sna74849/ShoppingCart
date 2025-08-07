namespace ShoppingCart.Models.Entity {
    public class DeliveryEntity {
        public string OrderCd { get; set; }
        public int BranchNo { get; set; }
        public int DestinationNo { get; set;}
        public DateTime DeliveryAt { get; set;}
        public DateTime SendAt { get; set;}
        public DateTime CreateAt { get; set; }
    }
}
