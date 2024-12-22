using E_Commerce.Core.Dtos.DeliveryMethodDto;
using MediatR;

namespace E_Commerce.Core.Commands.DeliveryMethodCommand
{
    public class UpdateDeliveryMethodCommand : IRequest<DeliveryMethodResponse>
    {
        public DeliveryMethodUpdateRequest DeliveryMethod { get; }

        public UpdateDeliveryMethodCommand(DeliveryMethodUpdateRequest deliveryMethod)
        {
            DeliveryMethod = deliveryMethod;
        }
    }
}
