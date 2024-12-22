using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.Dtos.CategoryDto
{
    public class CategoryAddRequest : CategoryBase
    {
        public IFormFile? CategoryImage { get; set; }
        public Guid? ParentCategoryID { get; set; }
    }
}
