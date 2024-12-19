using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.DeliveryMethodDto;

namespace E_Commerce.Core.MappingProfile
{
    public class DeliveryMethodConfig : Profile
    {
        public DeliveryMethodConfig()
        {
            CreateMap<DeliveryMethodAddRequest, DeliveryMethod>();
            CreateMap<DeliveryMethod, DeliveryMethodResponse>();
            CreateMap<DeliveryMethodUpdateRequest, DeliveryMethod>();

        }
    }
}
