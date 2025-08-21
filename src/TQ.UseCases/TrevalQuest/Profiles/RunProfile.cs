using AutoMapper;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Profiles
{
    public class RunProfile : Profile
    {
        public RunProfile()
        {
            CreateMap<Run, RunDTO>()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.Id.Value)
               )
               .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name)
               )
               .ForMember(
                    dest => dest.RunStart,
                    opt => opt.MapFrom(src => src.RunStart)
               )
               .ForMember(
                    dest => dest.Duration,
                    opt => opt.MapFrom(src => src.Duration)
               )
               .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description)
               )
               ;
        }
    }
}
