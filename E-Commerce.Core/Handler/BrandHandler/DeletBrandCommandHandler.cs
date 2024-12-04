using E_Commerce.Core.Commands.BrandCommand;
using E_Commerce.Core.Dtos.BrandDto;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.BrandHandler
{
    public class DeletBrandCommandHandler : IRequestHandler<DeleteBrandCommand, bool>
    {
        private readonly IBrandService _brandService;

        public DeletBrandCommandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<bool> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            return await _brandService.DeleteAsync(request.BrandID);
        }
    }
}
