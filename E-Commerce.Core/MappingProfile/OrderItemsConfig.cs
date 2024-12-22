using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.OrderItemsDto;

namespace E_Commerce.Core.MappingProfile
{
    public class OrderItemsConfig : Profile
    {
        public OrderItemsConfig()
        {
            CreateMap<OrderItemAddRequest,OrderItem>()
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(dest => dest.OrderItemID, opt => opt.MapFrom(src => src.OrderItemID))
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.Product.ProductImages.FirstOrDefault().ImageURL))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ReverseMap();


        }
    }
}
