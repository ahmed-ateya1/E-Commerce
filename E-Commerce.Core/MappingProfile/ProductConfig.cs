using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.ProductDto;

public class ProductConfig : Profile
{
    public ProductConfig()
    {
        CreateMap<ProductAddRequest, Product>()
            .ForMember(x => x.StockQuantityBeforeOrder, opt => opt.MapFrom(x => x.StockQuantity))
            .ReverseMap();

        CreateMap<ProductUpdateRequest, Product>()
            .ForMember(x => x.StockQuantityBeforeOrder, opt => opt.MapFrom(x => x.StockQuantity))
            .ReverseMap();

        CreateMap<Product, ProductResponse>()
            .ForMember(x => x.AvgRating, opt => opt.MapFrom(x => x.Reviews != null && x.Reviews.Any() ? x.Reviews.Average(r => r.Rating) : 0))
            .ForMember(x => x.TotalReviews, opt => opt.MapFrom(x => x.Reviews != null ? x.Reviews.Count : 0))
            .ForMember(x => x.BrandName, opt => opt.MapFrom(x => x.Brand != null ? x.Brand.BrandName : string.Empty))
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category != null ? x.Category.CategoryName : string.Empty))
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.User != null ? x.User.UserName : string.Empty))
            .ForMember(x => x.ProductPriceAfterDiscount, opt => opt.MapFrom(x => x.CalculateDiscountedPrice()))
            .ForMember(x => x.ProductFilesUrl, opt => opt.MapFrom(x => x.ProductImages.Select(p => p.ImageURL).ToList()))
            .ForMember(x => x.IsInStock, opt => opt.MapFrom(x => x.IsInStock()));
    }
}
