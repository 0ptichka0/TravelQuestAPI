using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateTeamCommand : IRequest<Result<TeamDTO>>
    {
        public UpdateTeamCommand(int id, TeamDTO model)
        {
            Id = id;
            Model = model;
        }
        public int Id { get; private set; }
        public TeamDTO Model { get; private set; }
    }
}
