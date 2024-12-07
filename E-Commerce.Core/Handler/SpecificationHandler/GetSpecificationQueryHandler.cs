using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using E_Commerce.Core.Queries.SpecificationQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.SpecificationHandler
{
    public class GetSpecificationQueryHandler : IRequestHandler<GetSpecificationQuery, TechnicalSpecificationResponse>
    {
        private readonly ICacheService _cacheService;
        private readonly ITechnicalSpecificationService _technicalSpecificationService;

        public GetSpecificationQueryHandler(ICacheService cacheService, ITechnicalSpecificationService technicalSpecificationService)
        {
            _cacheService = cacheService;
            _technicalSpecificationService = technicalSpecificationService;
        }

        public async Task<TechnicalSpecificationResponse> Handle(GetSpecificationQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync($"specification{request.SpecificationID}", async () =>
            {
                return await _technicalSpecificationService
                .GetByAsync(x=>x.TechnicalSpecificationID == request.SpecificationID);

            }, cancellationToken);
        }
    }
}
