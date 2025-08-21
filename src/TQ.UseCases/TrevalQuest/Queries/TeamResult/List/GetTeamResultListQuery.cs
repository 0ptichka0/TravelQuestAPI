using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.TeamResult.List
{
    public class GetTeamResultListQuery : IRequest<Result<IEnumerable<TeamResultDTO>>>
    {
        public GetTeamResultListQuery()
        {

        }
    }
}
