using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using MediatR;

namespace E_Commerce.Core.Commands.SpecificationCommand
{
    public class CreateSpecificationCommand : IRequest<TechnicalSpecificationResponse>
    {
        public TechnicalSpecificationAddRequest TechnicalSpecificationAddRequest { get;}

        public CreateSpecificationCommand(TechnicalSpecificationAddRequest technicalSpecificationAddRequest)
        {
            TechnicalSpecificationAddRequest = technicalSpecificationAddRequest;
        }
    }
}
