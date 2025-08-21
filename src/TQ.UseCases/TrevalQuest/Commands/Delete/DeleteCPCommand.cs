using Ardalis.Result;
using MediatR;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteCPCommand : IRequest<Result>
    {
        public DeleteCPCommand(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
    }
}
