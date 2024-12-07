using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using MediatR;

namespace E_Commerce.Core.Queries.SpecificationQueries
{
    public class GetSpecificationQuery : IRequest<TechnicalSpecificationResponse>
    {
        public Guid SpecificationID { get;}

        public GetSpecificationQuery(Guid specificationID)
        {
            SpecificationID = specificationID;
        }
    }
}
