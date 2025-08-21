using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.Team.List
{
    public class GetTeamListQuery : IRequest<Result<IEnumerable<TeamDTO>>>
    {
        public GetTeamListQuery()
        {

        }
    }
}
