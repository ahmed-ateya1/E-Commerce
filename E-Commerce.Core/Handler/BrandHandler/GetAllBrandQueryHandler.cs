﻿using E_Commerce.Core.Dtos.BrandDto;
using E_Commerce.Core.Queries.BrandQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.BrandHandler
{
    public class GetAllBrandQueryHandler : IRequestHandler<GetAllBrandQuery, IEnumerable<BrandResponse>>
    {
        private readonly IBrandService _brandService;

        public GetAllBrandQueryHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IEnumerable<BrandResponse>> Handle(GetAllBrandQuery request, CancellationToken cancellationToken)
        {
           return await _brandService
                .GetAllAsync(
               filter: request.Filter,
               pageSize: request.Pagination.PageSize,
               pageIndex: request.Pagination.PageIndex
               );
        }
    }
}
