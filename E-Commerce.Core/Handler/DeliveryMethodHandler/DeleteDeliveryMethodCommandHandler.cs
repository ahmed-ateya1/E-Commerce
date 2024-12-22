using E_Commerce.Core.Commands.DeliveryMethodCommand;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.DeliveryMethodHandler
{
    public class DeleteDeliveryMethodCommandHandler : IRequestHandler<DeleteDeliveryMethodCommand, bool>
    {
        private readonly IDeliveryMethodServices _deliveryMethodServices;
        public DeleteDeliveryMethodCommandHandler(IDeliveryMethodServices deliveryMethodServices)
        {
            _deliveryMethodServices = deliveryMethodServices;
        }

        public async Task<bool> Handle(DeleteDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            return await _deliveryMethodServices.DeleteAsync(request.ID);
        }
    }
}
