using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.VoteDto;

namespace E_Commerce.Core.MappingProfile
{
    public class VoteConfig : Profile
    {
        public VoteConfig()
        {
            CreateMap<VoteAddRequest, Vote>();

            CreateMap<Vote, VoteResponse>()
                .ForMember(dest => dest.VoteID, opt => opt.MapFrom(src => src.VoteID))
                .ForMember(dest => dest.ReviewID, opt => opt.MapFrom(src => src.ReviewID))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.VoteType, opt => opt.MapFrom(src => src.VoteType.ToString()));

            CreateMap<Vote, VoteUpdateRequest>();
        }
    }
}
