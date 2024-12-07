using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using MediatR;

namespace E_Commerce.Core.Queries.SpecificationQueries
{
    public class GetAllSpecificationsForProductQuery : IRequest<IEnumerable<TechnicalSpecificationResponse>>
    {
        public Guid ProductID { get;}

        public GetAllSpecificationsForProductQuery(Guid productID)
        {
            ProductID = productID;
        }
    }
}
