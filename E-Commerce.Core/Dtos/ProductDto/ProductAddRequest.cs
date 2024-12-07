using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.ProductDto
{
    public class ProductAddRequest : ProductBase
    {
        [Required(ErrorMessage = "must select at least one image")]
        public List<IFormFile> ProductFiles { get; set; }
    }
}
