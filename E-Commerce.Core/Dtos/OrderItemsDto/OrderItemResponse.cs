namespace E_Commerce.Core.Dtos.OrderItemsDto
{
    public class OrderItemResponse
    {
        public Guid OrderItemID { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; } // From Product
        public string ProductImage { get; set; } // Optional, from Product
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
