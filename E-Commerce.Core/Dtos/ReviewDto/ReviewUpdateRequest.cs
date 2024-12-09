using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.ReviewDto
{
    public class ReviewUpdateRequest : ReviewBase
    {
        [Required(ErrorMessage = "Review ID is required")]
        public Guid ReviewID { get; set; }
    }
}
