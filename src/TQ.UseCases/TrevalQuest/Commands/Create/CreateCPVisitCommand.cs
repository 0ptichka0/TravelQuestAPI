using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateCPVisitCommand : IRequest<Result<CPVisitDTO>>
    {
        public CreateCPVisitCommand(CPVisitDTO model)
        {
            Model = model;
        }
        public CPVisitDTO Model { get; set; }
    }
}
