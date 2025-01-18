using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.DealDto;

public class DealConfig : Profile
{
    public DealConfig()
    {
        CreateMap<DealAddRequest, Deal>();
        CreateMap<DealUpdateRequest, Deal>();

        CreateMap<Deal, DealResponse>()
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.Product.ProductID))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.ProductDescription))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.ProductPrice))
            .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ProductImages != null && src.Product.ProductImages.Any() ? src.Product.ProductImages.First().ImageURL : null))
            .ForMember(dest => dest.AvgRating, opt => opt.MapFrom(src => src.Product.Reviews != null && src.Product.Reviews.Any() ? src.Product.Reviews.Average(x => x.Rating) : 0))
            .ForMember(dest => dest.TotalReviews, opt => opt.MapFrom(src => src.Product.Reviews != null ? src.Product.Reviews.Count : 0))
            .ForMember(dest => dest.TotalOrders, opt => opt.MapFrom(src => src.Product.OrderItems != null ? src.Product.OrderItems.Count : 0))
            .ForMember(dest => dest.IsInStock, opt => opt.MapFrom(src => src.Product.IsInStock()))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActiveDeal()))
            .ForMember(dest => dest.ProductPriceAfterDiscount, opt => opt.MapFrom(src => src.PriceAfterDiscount()));
    }
}