using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.Team.Get
{
    public class GetTeamQuery : IRequest<Result<TeamDTO>>
    {
        public GetTeamQuery(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
