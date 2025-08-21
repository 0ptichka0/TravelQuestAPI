using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CPRun.Get
{
    public class GetCPRunQuery : IRequest<Result<CPRunDTO>>
    {
        public GetCPRunQuery(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
