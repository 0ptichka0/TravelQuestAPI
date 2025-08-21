using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateRunCommand : IRequest<Result<RunDTO>>
    {
        public CreateRunCommand(RunDTO model)
        {
            Model = model;
        }
        public RunDTO Model { get; set; }
    }
}
