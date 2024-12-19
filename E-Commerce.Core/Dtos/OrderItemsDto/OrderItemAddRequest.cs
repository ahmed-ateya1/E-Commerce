using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.OrderItemsDto
{
    public class OrderItemAddRequest
    {
        [Required]
        public Guid ProductID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; } 

    }
}
