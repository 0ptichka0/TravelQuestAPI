using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CPVisit.List
{
    public class GetCPVisitListQuery : IRequest<Result<IEnumerable<CPVisitDTO>>>
    {
        public GetCPVisitListQuery()
        {

        }
    }
}
