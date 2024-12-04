using E_Commerce.Core.Commands.BrandCommand;
using E_Commerce.Core.Dtos.BrandDto;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.BrandHandler
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, BrandResponse>
    {
        private readonly IBrandService _brandService;

        public UpdateBrandCommandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<BrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            return await _brandService.UpdateAsync(request.Brand);
        }
    }
}
