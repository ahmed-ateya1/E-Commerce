using E_Commerce.Core.Commands.SpecificationCommand;
using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.SpecificationHandler
{
    public class CreateSpecificationCommandHandler : IRequestHandler<CreateSpecificationCommand, TechnicalSpecificationResponse>
    {
        private readonly ITechnicalSpecificationService _technicalSpecificationService;

        public CreateSpecificationCommandHandler(ITechnicalSpecificationService technicalSpecificationService)
        {
            _technicalSpecificationService = technicalSpecificationService;
        }

        public async Task<TechnicalSpecificationResponse> Handle(CreateSpecificationCommand request, CancellationToken cancellationToken)
        {
            return await _technicalSpecificationService.CreateAsync(request.TechnicalSpecificationAddRequest);
        }
    }
}
