using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.CategoryDto
{
    public class CategoryBase
    {
        [Required(ErrorMessage = "Category Name Can't be Blank !")]
        [StringLength(50, ErrorMessage = "Category Name Can't be more than 50 characters")]
        public string CategoryName { get; set; }
    }
}
