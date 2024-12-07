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
    public class GetProductsByCategoryNameQuery : IRequest<PaginatedResponse<ProductResponse>>
    {
        public GetProductsByCategoryNameQuery(string categoryName, PaginationDto pagination)
        {
            CategoryName = categoryName;
            Pagination = pagination;
        }

        public string CategoryName { get; }
        public PaginationDto Pagination { get;}
    }
}
