using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateCPVisitCommand : IRequest<Result<CPVisitDTO>>
    {
        public UpdateCPVisitCommand(int id, CPVisitDTO model)
        {
            Id = id;
            Model = model;
        }
        public int Id { get; private set; }
        public CPVisitDTO Model { get; private set; }
    }
}
