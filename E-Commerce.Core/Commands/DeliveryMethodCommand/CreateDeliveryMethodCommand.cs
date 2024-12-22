using E_Commerce.Core.Dtos.DeliveryMethodDto;
using MediatR;

namespace E_Commerce.Core.Commands.DeliveryMethodCommand
{
    public class CreateDeliveryMethodCommand : IRequest<DeliveryMethodResponse>
    {
        public DeliveryMethodAddRequest DeliveryMethod { get; }

        public CreateDeliveryMethodCommand(DeliveryMethodAddRequest deliveryMethod)
        {
            DeliveryMethod = deliveryMethod;
        }
    }
}
