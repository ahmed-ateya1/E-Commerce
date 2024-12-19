using AutoMapper;
using E_Commerce.Core.Dtos.OrderDto;

namespace E_Commerce.Core.MappingProfile
{
    public class OrderConfig : Profile
    {
        public OrderConfig()
        {
            CreateMap<OrderAddRequest, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.GetPriceTotal()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}
