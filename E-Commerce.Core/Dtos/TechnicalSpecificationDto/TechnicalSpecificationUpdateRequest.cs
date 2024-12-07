using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.TechnicalSpecificationDto
{
    public class TechnicalSpecificationUpdateRequest : TechnicalSpecificationBase
    {
        [Required(ErrorMessage = "Technical Specification ID can't be blank.")]
        public Guid TechnicalSpecificationID { get; set; }
    }
}
