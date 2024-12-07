using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using E_Commerce.Core.Queries.SpecificationQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.SpecificationHandler
{
    public class GetAllSpecificationsForProductQueryHandler : IRequestHandler<GetAllSpecificationsForProductQuery, IEnumerable<TechnicalSpecificationResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly ITechnicalSpecificationService _technicalSpecificationService;

        public GetAllSpecificationsForProductQueryHandler(ICacheService cacheService, ITechnicalSpecificationService technicalSpecificationService)
        {
            _cacheService = cacheService;
            _technicalSpecificationService = technicalSpecificationService;
        }

        public async Task<IEnumerable<TechnicalSpecificationResponse>> Handle(GetAllSpecificationsForProductQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync($"Specifications-{request.ProductID}", async () =>
            {
                return await _technicalSpecificationService.GetAllAsync(x => x.ProductID == request.ProductID);
            });
        }
    }
}
