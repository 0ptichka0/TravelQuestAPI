using Ardalis.Result;
using MediatR;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteTeamResultCommand : IRequest<Result>
    {
        public DeleteTeamResultCommand(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
