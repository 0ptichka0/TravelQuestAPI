using Ardalis.Result;
using MediatR;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteTeamCommand : IRequest<Result>
    {
        public DeleteTeamCommand(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
