using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CPRun.List
{
    public class GetCPRunListQuery : IRequest<Result<IEnumerable<CPRunDTO>>>
    {
        public GetCPRunListQuery()
        {

        }
    }
}
