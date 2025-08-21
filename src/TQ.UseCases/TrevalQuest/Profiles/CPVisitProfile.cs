using AutoMapper;
using TQ.Core.Aggregates.CPVisitsAggregate;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Profiles
{
    public class CPVisitProfile : Profile
    {
        public CPVisitProfile()
        {
            CreateMap<CPVisit, CPVisitDTO>()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.Id.Value)
               )
               .ForMember(
                    dest => dest.TeamId,
                    opt => opt.MapFrom(src => src.TeamId.Value)
               )
               .ForMember(
                    dest => dest.CPId,
                    opt => opt.MapFrom(src => src.CPId.Value)
               )
               .ForMember(
                    dest => dest.IsValid,
                    opt => opt.MapFrom(src => src.IsValid)
               )
               .ForMember(
                    dest => dest.VisitTime,
                    opt => opt.MapFrom(src => src.VisitTime)
               )
               ;
        }
    }
}
