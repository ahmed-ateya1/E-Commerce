using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.WishlistDto;

namespace E_Commerce.Core.MappingProfile
{
    public class WishlistConfig : Profile
    {
        public WishlistConfig()
        {
            CreateMap<WishlistAddRequest, Wishlist>();
            CreateMap<WishlistUpdateRequest, Wishlist>();
            CreateMap<Wishlist, WishlistResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest=>dest.ProductPrice, opt=> opt.MapFrom(src => src.Product.CalculateDiscountedPrice()))
.ForMember(dest => dest.ProductImageURL, opt => opt.MapFrom(src => src.Product.ProductImages.FirstOrDefault().ImageURL))
                .ForMember(dest=>dest.ProductDescription, opt => opt.MapFrom(src => src.Product.ProductDescription))
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.Product.CategoryID))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Category.CategoryName))
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.AddedAt));

        }
    }
}
