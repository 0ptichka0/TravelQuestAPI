using Ardalis.Result;
using MediatR;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteCPRunCommand : IRequest<Result>
    {
        public DeleteCPRunCommand(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
