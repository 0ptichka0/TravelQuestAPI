using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateRunCommand : IRequest<Result<RunDTO>>
    {
        public UpdateRunCommand(string id, RunDTO model)
        {
            Id = id;
            Model = model;
        }
        public string Id { get; private set; }
        public RunDTO Model { get; private set; }
    }
}
