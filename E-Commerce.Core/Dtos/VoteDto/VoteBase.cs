using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.VoteDto
{
    public class VoteBase 
    {
        [Required(ErrorMessage = "ReviewID is required")]
        public Guid ReviewID { get; set; }
    }
}
