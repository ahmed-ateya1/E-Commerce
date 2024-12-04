using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.BrandDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.MappingProfile
{
    public class BrandConfig : Profile
    {
        public BrandConfig()
        {
            CreateMap<BrandAddRequest, Brand>()
                .ForMember(dest =>dest.BrandName, opt => opt.MapFrom(src => src.BrandName));
            CreateMap<BrandUpdateRequest, Brand>();
            CreateMap<Brand, BrandResponse>();
        }
    }
}
