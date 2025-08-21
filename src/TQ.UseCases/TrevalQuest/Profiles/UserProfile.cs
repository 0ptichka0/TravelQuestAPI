using AutoMapper;
using TQ.Core.Aggregates.UsersAggregate;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.Id.Value)
               )
               .ForMember(
                    dest => dest.TeamId,
                    opt => opt.MapFrom(src => src.TeamId.Value)
               )
               .ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.FirstName)
               )
               .ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(src => src.LastName)
               )
               .ForMember(
                    dest => dest.Code,
                    opt => opt.MapFrom(src => src.Code)
               )
               ;
        }
    }
}
