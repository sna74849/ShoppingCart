using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models.Entity {
    public class CustomerEntity {
        [Column("customer_id")]
        public string CustomerId { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public DateTime UpdatedAt { get; set; } = default!;
    }
}
