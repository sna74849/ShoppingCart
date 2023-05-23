namespace ShoppingCart.Models.Dto
{
    public class ItemDto
    {
        public string JanCd { get; set; }
        public string ItemCd { get; set; }
        public string ItemNm { get; set; }
        public int Price { get; set; }
        public string FilePath { get; set; }
        public int Amount { get; set; }
        public int Qty { get; set; }
    }
}
