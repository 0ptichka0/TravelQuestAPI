using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.Run.List
{
    public class GetRunListQuery : IRequest<Result<IEnumerable<RunDTO>>>
    {
        public GetRunListQuery()
        {

        }
    }
}
