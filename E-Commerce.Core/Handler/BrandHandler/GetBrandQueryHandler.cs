using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.BrandDto;
using E_Commerce.Core.Queries.BrandQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Handler.BrandHandler
{
    public class GetBrandQueryHandler : IRequestHandler<GetBrandQuery, BrandResponse>
    {
        private readonly IBrandService _brandService;
        private readonly ICacheService _cacheService;

        public GetBrandQueryHandler(IBrandService brandService, ICacheService cacheService)
        {
            _brandService = brandService;
            _cacheService = cacheService;
        }

        public async Task<BrandResponse> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync("brand_" + request.BrandID, async () =>
            {
                return await _brandService
                .GetByAsync(x => x.BrandID == request.BrandID);
            }, cancellationToken);
        }
    }
}
