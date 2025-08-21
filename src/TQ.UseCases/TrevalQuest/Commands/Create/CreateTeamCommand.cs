using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateTeamCommand : IRequest<Result<TeamDTO>>
    {
        public CreateTeamCommand(TeamDTO model)
        {
            Model = model;
        }
        public TeamDTO Model { get; set; }
    }
}
