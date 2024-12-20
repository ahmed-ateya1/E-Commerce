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
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ReverseMap();

        }
    }
}
