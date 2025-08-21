using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateCPRunCommand : IRequest<Result<CPRunDTO>>
    {
        public UpdateCPRunCommand(int id, CPRunDTO model)
        {
            Id = id;
            Model = model;
        }
        public int Id { get; private set; }
        public CPRunDTO Model { get; private set; }
    }
}
