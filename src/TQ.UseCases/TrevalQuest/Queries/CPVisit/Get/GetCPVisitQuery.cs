using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CPVisit.Get
{
    public class GetCPVisitQuery : IRequest<Result<CPVisitDTO>>
    {
        public GetCPVisitQuery(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
