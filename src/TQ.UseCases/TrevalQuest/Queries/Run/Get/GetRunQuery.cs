using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.Run.Get
{
    public class GetRunQuery : IRequest<Result<RunDTO>>
    {
        public GetRunQuery(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
    }
}
