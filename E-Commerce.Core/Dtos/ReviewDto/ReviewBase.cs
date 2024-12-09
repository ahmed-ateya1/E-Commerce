using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.ReviewDto
{
    public class ReviewBase 
    {
        [Required(ErrorMessage = "Review text is required")]
        [StringLength(500, ErrorMessage = "Review text cannot be more than 500 characters")]
        public string ReviewText { get; set; }
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Product ID is required")]
        public Guid ProductID { get; set; }
    }
}
