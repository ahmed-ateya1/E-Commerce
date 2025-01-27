namespace E_Commerce.Core.Dtos.OrderItemsDto
{
    public class OrderItemAddRequest
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } 

    }
}
