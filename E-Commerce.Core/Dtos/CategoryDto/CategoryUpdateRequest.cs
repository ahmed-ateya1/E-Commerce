using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.CategoryDto
{
    public class CategoryUpdateRequest : CategoryBase
    {
        [Required(ErrorMessage = "Category ID Can't be Blank !")]
        public Guid CategoryID { get; set; }
        public IFormFile? CategoryImage { get; set; }
        
        public Guid? ParentCategoryID { get; set; }
    }
}
