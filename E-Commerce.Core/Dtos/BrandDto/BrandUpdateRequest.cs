using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.BrandDto
{
    public class BrandUpdateRequest : BrandBase
    {
        [Required(ErrorMessage = "Brand ID can't be blank.")]
        public Guid BrandID { get; set; }
    }
}
