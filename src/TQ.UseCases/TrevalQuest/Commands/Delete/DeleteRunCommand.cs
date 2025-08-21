using Ardalis.Result;
using MediatR;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteRunCommand : IRequest<Result>
    {
        public DeleteRunCommand(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
    }
}
