using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using MediatR;

namespace E_Commerce.Core.Commands.SpecificationCommand
{
    public class UpdateSpecificationCommand : IRequest<TechnicalSpecificationResponse>
    {
        public TechnicalSpecificationUpdateRequest _request { get; }

        public UpdateSpecificationCommand(TechnicalSpecificationUpdateRequest request)
        {
            _request = request;
        }
    }
}
