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
    public class GetProductsByRatingQuery : IRequest<PaginatedResponse<ProductResponse>>
    {
        public GetProductsByRatingQuery(int rating, PaginationDto pagination)
        {
            Rating = rating;
            Pagination = pagination;
        }
        public int Rating { get; }
        public PaginationDto Pagination { get; }
    }
}
