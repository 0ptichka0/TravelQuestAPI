using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateCPCommand : IRequest<Result<CPDTO>>
    {
        public CreateCPCommand(CPDTO model)
        {
            Model = model;
        }
        public CPDTO Model { get; set; }
    }
}
