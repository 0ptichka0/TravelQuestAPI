using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateTeamResultCommand : IRequest<Result<TeamResultDTO>>
    {
        public CreateTeamResultCommand(TeamResultDTO model)
        {
            Model = model;
        }
        public TeamResultDTO Model { get; set; }
    }
}
