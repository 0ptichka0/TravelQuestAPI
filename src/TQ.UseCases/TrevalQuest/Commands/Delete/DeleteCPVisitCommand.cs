using Ardalis.Result;
using MediatR;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteCPVisitCommand : IRequest<Result>
    {
        public DeleteCPVisitCommand(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
