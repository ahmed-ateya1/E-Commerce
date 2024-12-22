using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.OrderItemsDto
{
    public class OrderItemAddRequest
    {
        [Required]
        public Guid ProductID { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; } 

    }
}
