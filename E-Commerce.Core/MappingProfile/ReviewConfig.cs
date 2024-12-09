using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.ReviewDto;

namespace E_Commerce.Core.MappingProfile
{
    public class ReviewConfig : Profile
    {
        public ReviewConfig()
        {
            CreateMap<ReviewAddRequest, Review>();
            CreateMap<Review, ReviewUpdateRequest>();

            CreateMap<Review, ReviewResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.TotalRepliesReviews, opt => opt.MapFrom(src => src.ChildReviews.Count));
        }
    }
}
