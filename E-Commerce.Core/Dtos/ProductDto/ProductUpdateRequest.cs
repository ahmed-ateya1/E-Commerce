using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Dtos.ProductDto
{
    public class ProductUpdateRequest : ProductBase
    {
        [Required(ErrorMessage = "Product ID can't be blank.")]
        public Guid ProductID { get; set; }
        
        public List<IFormFile>? ProductFiles { get; set; }
    }
}
