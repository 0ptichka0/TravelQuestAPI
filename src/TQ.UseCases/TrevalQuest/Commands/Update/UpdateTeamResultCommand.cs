using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateTeamResultCommand : IRequest<Result<TeamResultDTO>>
    {
        public UpdateTeamResultCommand(int id, TeamResultDTO model)
        {
            Id = id;
            Model = model;
        }
        public int Id { get; private set; }
        public TeamResultDTO Model { get; private set; }
    }
}
