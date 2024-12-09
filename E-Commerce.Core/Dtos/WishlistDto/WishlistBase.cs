using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.WishlistDto
{
    public class WishlistBase 
    {
        [Required(ErrorMessage = "Product ID can't be blank.")]
        public Guid ProductID { get; set; }
    }
}
