namespace ShoppingCart.Models.Entity {
    public class StockEntity {
        public int StockNo { get; set; }
        public string JanCd { get; set; }
        public string? OrderCd { get; set; }
        public int? BranchNo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
