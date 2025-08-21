using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateCPRunCommand : IRequest<Result<CPRunDTO>>
    {
        public CreateCPRunCommand(CPRunDTO model)
        {
            Model = model;
        }
        public CPRunDTO Model { get; set; }
    }
}
