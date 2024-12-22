using E_Commerce.Core.Dtos.AddressDto;
using E_Commerce.Core.Dtos.DeliveryMethodDto;
using E_Commerce.Core.Dtos.OrderItemsDto;

namespace E_Commerce.Core.Dtos.OrderDto
{
    public class OrderResponse
    {
        public Guid OrderID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; } 
        public string ClientSecret { get; set; }
        public string PaymentIntentID { get; set; }
        public Guid AddressID { get; set; }
        public AddressResponse Address { get; set; }
        public Guid DeliveryMethodID { get; set; }
        public DeliveryMethodResponse DeliveryMethod { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } 
    }

}
