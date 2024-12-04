using E_Commerce.Core.Dtos.BrandDto;
using MediatR;

namespace E_Commerce.Core.Commands.BrandCommand
{
    public class UpdateBrandCommand : IRequest<BrandResponse>
    {
        public BrandUpdateRequest Brand { get; set; }
        public UpdateBrandCommand(BrandUpdateRequest brand)
        {
            Brand = brand;
        }
    }
}
