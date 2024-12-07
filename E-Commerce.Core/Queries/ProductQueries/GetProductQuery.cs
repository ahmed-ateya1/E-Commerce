using E_Commerce.Core.Dtos.ProductDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Queries.ProductQueries
{
    public class GetProductQuery : IRequest<ProductResponse>
    {
        public Guid Id { get; set; }
    }
}
