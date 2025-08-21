using AutoMapper;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Profiles
{
    public class CPRunProfile : Profile
    {
        public CPRunProfile()
        {
            CreateMap<CPRun, CPRunDTO>()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.Id.Value)
               )
               .ForMember(
                    dest => dest.RunId,
                    opt => opt.MapFrom(src => src.RunId.Value)
               )
               .ForMember(
                    dest => dest.CPId,
                    opt => opt.MapFrom(src => src.CPId.Value)
               )
               .ForMember(
                    dest => dest.Scores,
                    opt => opt.MapFrom(src => src.Scores)
               )
               ;
        }
    }
}
