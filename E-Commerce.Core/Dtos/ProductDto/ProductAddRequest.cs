using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.Dtos.ProductDto
{
    public class ProductAddRequest : ProductBase
    {
        public List<IFormFile> ProductFiles { get; set; }
    }
}
