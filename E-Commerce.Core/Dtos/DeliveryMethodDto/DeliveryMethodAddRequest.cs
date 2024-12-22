using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.DeliveryMethodDto
{
    public class DeliveryMethodAddRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Short Name can't be longer than 50 characters.")]
        public string ShortName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Delivery Time can't be longer than 50 characters.")]
        public string DeliveryTime { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Description can't be longer than 100 characters.")]
        public string Description { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}
