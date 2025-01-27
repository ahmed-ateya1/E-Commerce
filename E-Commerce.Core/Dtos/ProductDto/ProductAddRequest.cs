using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.Dtos.ProductDto
{
    public class ProductAddRequest : ProductBase
    {
        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only .jpg, .jpeg, and .png files are allowed.")]
        public List<IFormFile> ProductFiles { get; set; }
    }
}
