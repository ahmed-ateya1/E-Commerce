using E_Commerce.Core.Commands.SpecificationCommand;
using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.SpecificationHandler
{
    public class UpdateSpecificationCommandHandler : IRequestHandler<UpdateSpecificationCommand, TechnicalSpecificationResponse>
    {
        private readonly ITechnicalSpecificationService _technicalSpecificationService;

        public UpdateSpecificationCommandHandler(ITechnicalSpecificationService technicalSpecificationService)
        {
            _technicalSpecificationService = technicalSpecificationService;
        }

        public async Task<TechnicalSpecificationResponse> Handle(UpdateSpecificationCommand request, CancellationToken cancellationToken)
        {
            return await _technicalSpecificationService.UpdateAsync(request._request);
        }
    }
}
