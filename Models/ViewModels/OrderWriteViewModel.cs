namespace ShoppingCart.Models.ViewModels
{
    public class OrderWriteViewModel
    {
        public int DestinationNo { get; set; } = default!;

        public List<OrderScheduledDeliveryViewModel> OrderScheduledDeliveryVmList { get; set; } = default!;
    }
}
