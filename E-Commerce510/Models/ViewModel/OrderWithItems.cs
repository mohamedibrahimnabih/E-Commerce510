namespace E_Commerce510.Models.ViewModel
{
    public class OrderWithItems
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
