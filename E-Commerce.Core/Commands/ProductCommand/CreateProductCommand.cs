using E_Commerce.Core.Dtos.ProductDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Commands.ProductCommand
{
    public class CreateProductCommand : IRequest<ProductResponse>
    {
        public ProductAddRequest Product { get;}

        public CreateProductCommand(ProductAddRequest product)
        {
            Product = product;
        }
    }
}
