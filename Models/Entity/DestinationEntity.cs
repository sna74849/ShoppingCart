namespace ShoppingCart.Models.Entity {
    public class DestinationEntity {
        public int DestinationNo { get; set; } = default!;
        public string CustomerId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Postcode { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public DateTime UpdatedAt { get; set; } = default!;
    }
}
