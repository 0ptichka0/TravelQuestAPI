using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQ.Core.Aggregates.TeamsAggregate;
using TQ.Core.Aggregates.UsersAggregate;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Profiles
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, TeamDTO>()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.Id.Value)
               )
               .ForMember(
                    dest => dest.RunId,
                    opt => opt.MapFrom(src => src.RunId.Value)
               )
               .ForMember(
                    dest => dest.RegistrationDate,
                    opt => opt.MapFrom(src => src.RegistrationDate)
               )
               .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name)
               )
               .ForMember(
                    dest => dest.Code,
                    opt => opt.MapFrom(src => src.Code)
               )
               .ForMember(
                    dest => dest.Area,
                    opt => opt.MapFrom(src => src.Area)
               )
               .ForMember(
                    dest => dest.Group,
                    opt => opt.MapFrom(src => src.Group)
               )
               ;
        }
    }
}
