using E_Commerce.Core.Dtos.OrderItemsDto;

namespace E_Commerce.Core.Dtos.OrderDto
{
    public class OrderAddRequest
    {
        public string OrderNumber { get; set; }
        public decimal SubTotal { get; set; }
        public Guid UserID { get; set; }
        public Guid AddressID { get; set; }
        public Guid DeliveryMethodID { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
    }
}
