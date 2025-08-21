using AutoMapper;
using TQ.Core.Aggregates.CPsAggregate;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Profiles
{
    public class CPProfile : Profile
    {
        public CPProfile()
        {
            CreateMap<CP, CPDTO>()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.Id.Value)
               )
               .ForMember(
                    dest => dest.Number,
                    opt => opt.MapFrom(src => src.Number)
               )
               ;
        }
    }
}
