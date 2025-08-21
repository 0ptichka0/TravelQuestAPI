using AutoMapper;
using TQ.Core.Aggregates.TeamResultsAggregate;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Profiles
{
    public class TeamResultProfile : Profile
    {
        public TeamResultProfile()
        {
            CreateMap<TeamResult, TeamResultDTO>()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.Id.Value)
               )
               .ForMember(
                    dest => dest.TeamId,
                    opt => opt.MapFrom(src => src.TeamId.Value)
               )
               .ForMember(
                    dest => dest.TotalScore,
                    opt => opt.MapFrom(src => src.TotalScore)
               )
               .ForMember(
                    dest => dest.ElapsedTime,
                    opt => opt.MapFrom(src => src.ElapsedTime)
               )
               .ForMember(
                    dest => dest.Penalty,
                    opt => opt.MapFrom(src => src.Penalty)
               )
               ;
        }
    }
}
