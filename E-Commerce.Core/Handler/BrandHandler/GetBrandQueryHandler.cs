using E_Commerce.Core.Dtos.BrandDto;
using E_Commerce.Core.Queries.BrandQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Handler.BrandHandler
{
    public class GetBrandQueryHandler : IRequestHandler<GetBrandQuery, BrandResponse>
    {
        private readonly IBrandService _brandService;

        public GetBrandQueryHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<BrandResponse> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            return await _brandService
                .GetByAsync(x=>x.BrandID == request.BrandID);
        }
    }
}
