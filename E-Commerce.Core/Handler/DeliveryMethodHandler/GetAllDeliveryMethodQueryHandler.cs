using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.DeliveryMethodDto;
using E_Commerce.Core.Queries.DeliveryMethodQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.DeliveryMethodHandler
{
    public class GetAllDeliveryMethodQueryHandler : IRequestHandler<GetAllDeliveryMethodQuery, IEnumerable<DeliveryMethodResponse>>
    {
        private readonly IDeliveryMethodServices _deliveryMethodServices;
        private readonly ICacheService _cacheService;

        public GetAllDeliveryMethodQueryHandler(IDeliveryMethodServices deliveryMethodServices, ICacheService cacheService)
        {
            _deliveryMethodServices = deliveryMethodServices;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> Handle(GetAllDeliveryMethodQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync("DeliveryMethods", async () => await _deliveryMethodServices.GetAllAsync());
        }
    }
}
