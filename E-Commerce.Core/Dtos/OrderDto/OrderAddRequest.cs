using E_Commerce.Core.Dtos.OrderItemsDto;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.OrderDto
{
    public class OrderAddRequest
    {
        public Guid AddressID { get; set; }
        public Guid DeliveryMethodID { get; set; }
        public List<OrderItemAddRequest> OrderItems { get; set; } = new List<OrderItemAddRequest>();
    }
}
