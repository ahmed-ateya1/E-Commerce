namespace E_Commerce.Core.Domain.Entities
{
    public class OrderItem
    {
        public Guid OrderItemID { get; set; } = Guid.NewGuid();
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid OrderID { get; set; }
        public Order Order { get; set; }

        public Guid ProductID { get; set; }
        public Product Product { get; set; }
    }
}
