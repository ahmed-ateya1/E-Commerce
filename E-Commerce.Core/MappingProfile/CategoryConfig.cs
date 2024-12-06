using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.CategoryDto;

namespace E_Commerce.Core.MappingProfile
{
    public class CategoryConfig : Profile
    {
        public CategoryConfig()
        {
            CreateMap<CategoryAddRequest, Category>();

            CreateMap<CategoryUpdateRequest, Category>();

            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory.CategoryName));

        }
    }
}
