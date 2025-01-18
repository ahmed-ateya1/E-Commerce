using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.DealDto
{
    public class  DealUpdateRequest
    {
        [Required(ErrorMessage = "Deal can't be blank!")]
        public Guid DealID { get; set; }
        [Required(ErrorMessage = "Discount can't be blank!")]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100!")]
        public decimal Discount { get; set; }
        [Required(ErrorMessage = "Start date can't be blank!")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date can't be blank!")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Product can't be blank!")]
        public Guid ProductID { get; set; }
    }
}
