<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models.Entity {
    public class CustomerEntity {
        [Column("customer_id")]
        public string CustomerId { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public DateTime UpdatedAt { get; set; } = default!;
=======
﻿namespace ShoppingCart.Models.Entity {
    public class CustomerEntity {
        public string CustomerId { get; set; }
        public string Password { get; set; }
>>>>>>> 91decb47f3a5019fbd405ce4ab88d86b6396da4d
    }
}
