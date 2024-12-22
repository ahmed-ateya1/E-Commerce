using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.DeliveryMethodDto;
using E_Commerce.Core.Queries.DeliveryMethodQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.DeliveryMethodHandler
{
    public class GetDeliveryMethodByIdQueryHandler : IRequestHandler<GetDeliveryMethodByIdQuery, DeliveryMethodResponse>
    {
        private readonly IDeliveryMethodServices _deliveryMethodServices;
        private readonly ICacheService _cacheService;

        public GetDeliveryMethodByIdQueryHandler(IDeliveryMethodServices deliveryMethodServices, ICacheService cacheService)
        {
            _deliveryMethodServices = deliveryMethodServices;
            _cacheService = cacheService;
        }

        public async Task<DeliveryMethodResponse> Handle(GetDeliveryMethodByIdQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync<DeliveryMethodResponse>($"DeliveryMethod{request.ID}", async () =>
            {
                return await _deliveryMethodServices.GetByAsync(x=>x.DeliveryMethodID == request.ID);
            });
        }
    }
}
