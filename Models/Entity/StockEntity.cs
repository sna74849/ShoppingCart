namespace ShoppingCart.Models.Entity {
    public class StockEntity {
        public int StockNo { get; set; } = default!;
        public string JanCd { get; set; } = default!;
        public string? OrderCd { get; set; } = null;
        public int BranchNo { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public DateTime UpdatedAt { get; set; } = default!;
    }
}
