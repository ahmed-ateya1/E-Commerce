using MediatR;

namespace E_Commerce.Core.Commands.DeliveryMethodCommand
{
    public class DeleteDeliveryMethodCommand : IRequest<bool>
    {
        public Guid ID { get; set; }
    }
}
