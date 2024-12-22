using E_Commerce.Core.Commands.DeliveryMethodCommand;
using E_Commerce.Core.Dtos.DeliveryMethodDto;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.DeliveryMethodHandler
{
    public class CreateDeliveryMethodCommandHandler : IRequestHandler<CreateDeliveryMethodCommand, DeliveryMethodResponse>
    {
        private readonly IDeliveryMethodServices _deliveryMethodServices;

        public CreateDeliveryMethodCommandHandler(IDeliveryMethodServices deliveryMethodServices)
        {
            _deliveryMethodServices = deliveryMethodServices;
        }

        public async Task<DeliveryMethodResponse> Handle(CreateDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            return await _deliveryMethodServices.CreateAsync(request.DeliveryMethod);
        }
    }
}
