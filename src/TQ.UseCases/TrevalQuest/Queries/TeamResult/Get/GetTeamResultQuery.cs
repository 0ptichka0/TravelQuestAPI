using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.TeamResult.Get
{
    public class GetTeamResultQuery : IRequest<Result<TeamResultDTO>>
    {
        public GetTeamResultQuery(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
