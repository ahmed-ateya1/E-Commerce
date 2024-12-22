using E_Commerce.Core.Dtos.DeliveryMethodDto;
using MediatR;

namespace E_Commerce.Core.Queries.DeliveryMethodQueries
{
    public class GetDeliveryMethodByIdQuery : IRequest<DeliveryMethodResponse>
    {
        public Guid ID { get; set; }
    }
}
