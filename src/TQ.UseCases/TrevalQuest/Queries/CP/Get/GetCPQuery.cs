using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CP.Get
{
    public class GetCPQuery : IRequest<Result<CPDTO>>
    {
        public GetCPQuery(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
    }
}
