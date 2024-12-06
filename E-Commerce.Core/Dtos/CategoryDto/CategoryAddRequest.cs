using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Dtos.CategoryDto
{
    public class CategoryAddRequest : CategoryBase
    {
        [Required(ErrorMessage = "Category Image is Required !")]
        public IFormFile? CategoryImage { get; set; }
        public Guid? ParentCategoryID { get; set; }
    }
}
