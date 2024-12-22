using E_Commerce.Core.Commands.DeliveryMethodCommand;
using E_Commerce.Core.Dtos.DeliveryMethodDto;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.DeliveryMethodHandler
{
    public class UpdateDeliveryMethodCommandHandler : IRequestHandler<UpdateDeliveryMethodCommand, DeliveryMethodResponse>
    {
        private readonly IDeliveryMethodServices _deliveryMethodServices;

        public UpdateDeliveryMethodCommandHandler(IDeliveryMethodServices deliveryMethodServices)
        {
            _deliveryMethodServices = deliveryMethodServices;
        }

        public async Task<DeliveryMethodResponse> Handle(UpdateDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            return await _deliveryMethodServices.UpdateAsync(request.DeliveryMethod);
        }
    }
}
