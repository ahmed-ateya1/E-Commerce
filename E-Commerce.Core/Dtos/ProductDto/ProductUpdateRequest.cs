using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.Dtos.ProductDto
{
    public class ProductUpdateRequest : ProductBase
    {
        public Guid ProductID { get; set; }
        public List<IFormFile>? ProductFiles { get; set; }
    }
}
