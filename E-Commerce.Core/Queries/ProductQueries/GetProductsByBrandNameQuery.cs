using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Queries.ProductQueries
{
    public class GetProductsByBrandNameQuery : IRequest<PaginatedResponse<ProductResponse>>
    {
        public GetProductsByBrandNameQuery(string brandName, PaginationDto pagination)
        {
            BrandName = brandName;
            Pagination = pagination;
        }
        public string BrandName { get; }
        public PaginationDto Pagination
        {
            get;
        }
    }
}
