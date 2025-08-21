using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CP.List
{
    public class GetCPListQuery : IRequest<Result<IEnumerable<CPDTO>>>
    {
        public GetCPListQuery()
        {

        }
    }
}
