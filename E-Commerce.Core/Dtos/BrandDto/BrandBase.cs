using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.BrandDto
{
    public class BrandBase
    {
        [Required(ErrorMessage = "Brand Name can't be blank .")]
        [StringLength(50, ErrorMessage = "Brand Name can't be longer than 50 characters.")]
        public string BrandName { get; set; }
    }
}
