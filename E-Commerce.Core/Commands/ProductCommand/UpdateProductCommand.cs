using E_Commerce.Core.Dtos.ProductDto;
using MediatR;

namespace E_Commerce.Core.Commands.ProductCommand
{
    public class UpdateProductCommand : IRequest<ProductResponse>
    {
        public ProductUpdateRequest Product { get; }
        public UpdateProductCommand(ProductUpdateRequest product)
        {
            Product = product;
        }
    }
}
