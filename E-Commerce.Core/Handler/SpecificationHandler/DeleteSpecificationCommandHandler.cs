using E_Commerce.Core.Commands.SpecificationCommand;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.SpecificationHandler
{
    public class DeleteSpecificationCommandHandler : IRequestHandler<DeleteSpecificationCommand, bool>
    {
        private readonly ITechnicalSpecificationService _technicalSpecificationService;

        public DeleteSpecificationCommandHandler(ITechnicalSpecificationService technicalSpecificationService)
        {
            _technicalSpecificationService = technicalSpecificationService;
        }

        public async Task<bool> Handle(DeleteSpecificationCommand request, CancellationToken cancellationToken)
        {
            return await _technicalSpecificationService.DeleteAsync(request.Id);
        }
    }
}
