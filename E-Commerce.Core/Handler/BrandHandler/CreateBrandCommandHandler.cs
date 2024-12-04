using E_Commerce.Core.Commands.BrandCommand;
using E_Commerce.Core.Dtos.BrandDto;
using E_Commerce.Core.ServicesContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Handler.BrandHandler
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, BrandResponse>
    {
        private readonly IBrandService _brandService;

        public CreateBrandCommandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<BrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
           return await _brandService.CreateAsync(request.Brand);
        }
    }
}
