using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateCPCommand : IRequest<Result<CPDTO>>
    {
        public UpdateCPCommand(string id, CPDTO model)
        {
            Id = id;
            Model = model;
        }
        public string Id { get; private set; }
        public CPDTO Model { get; private set; }
    }
}
