using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.TechnicalSpecificationDto;

namespace E_Commerce.Core.MappingProfile
{
    public class TechnicalSpecificationConfig : Profile
    {
        public TechnicalSpecificationConfig()
        {
            CreateMap<TechnicalSpecificationAddRequest, TechnicalSpecification>();
            CreateMap<TechnicalSpecificationUpdateRequest, TechnicalSpecification>();
            CreateMap<TechnicalSpecification, TechnicalSpecificationResponse>();
        }
    }
}
