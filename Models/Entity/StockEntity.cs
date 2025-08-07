namespace ShoppingCart.Models.Entity {
    public class StockEntity {
<<<<<<< HEAD
        public int StockNo { get; set; } = default!;
        public string JanCd { get; set; } = default!;
        public string? OrderCd { get; set; } = null;
        public int BranchNo { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public DateTime UpdatedAt { get; set; } = default!;
=======
        public int StockNo { get; set; }
        public string JanCd { get; set; }
        public string? OrderCd { get; set; }
        public int? BranchNo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
>>>>>>> 91decb47f3a5019fbd405ce4ab88d86b6396da4d
    }
}
