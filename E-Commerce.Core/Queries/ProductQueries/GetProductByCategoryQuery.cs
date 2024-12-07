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
    public class GetProductByCategoryQuery : IRequest<PaginatedResponse<ProductResponse>>
    {
        public Guid CategoryId { get;}
        public PaginationDto Pagination { get;}

        public GetProductByCategoryQuery(Guid categoryId, PaginationDto pagination)
        {
            CategoryId = categoryId;
            Pagination = pagination;
        }
    }
}
