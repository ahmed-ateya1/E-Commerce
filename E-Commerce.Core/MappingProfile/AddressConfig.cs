using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.AddressDto;

namespace E_Commerce.Core.MappingProfile
{
    public class AddressConfig : Profile
    {
        public AddressConfig()
        {
            CreateMap<AddressAddRequest, Address>()
                .ReverseMap();
            CreateMap<Address, AddressResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();

        }
    }
}
