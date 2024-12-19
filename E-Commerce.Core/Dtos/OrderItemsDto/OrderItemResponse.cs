namespace E_Commerce.Core.Dtos.OrderItemsDto
{
    public class OrderItemResponse
    {
        public Guid OrderItemID { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; } 
        public string ProductImage { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
