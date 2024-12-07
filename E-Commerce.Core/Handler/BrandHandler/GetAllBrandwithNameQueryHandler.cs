using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.BrandDto;
using E_Commerce.Core.Queries.BrandQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.BrandHandler
{
    public class GetAllBrandwithNameQueryHandler : IRequestHandler<GetAllBrandwithNameQuery, IEnumerable<BrandResponse>>
    {
        private readonly IBrandService _brandService;
        private readonly ICacheService _cacheService;

        public GetAllBrandwithNameQueryHandler(IBrandService brandService, ICacheService cacheService)
        {
            _brandService = brandService;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<BrandResponse>> Handle(GetAllBrandwithNameQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync($"all_brands_{request.Name}", async () =>
            {
                return await _brandService
                .GetAllAsync(x => x.BrandName.ToUpper().Contains(request.Name.ToUpper()));
            }, cancellationToken);
        }
    }
}
