using E_Commerce.Core.Dtos.OrderItemsDto;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.OrderDto
{
    public class OrderAddRequest
    {
        [Required]
        public Guid AddressID { get; set; }
        [Required]
        public Guid DeliveryMethodID { get; set; }
        [Required]
        public List<OrderItemAddRequest> OrderItems { get; set; } = new List<OrderItemAddRequest>();
    }
}
