using Ardalis.Result;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteCPVisitHandler : IRequestHandler<DeleteCPVisitCommand, Result>
    {
        private readonly ICPVisitService _service;
        public DeleteCPVisitHandler(ICPVisitService service)
        {
            _service = service;
        }
        public async Task<Result> Handle(DeleteCPVisitCommand request, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(new IntId(request.Id));
        }
    }
}
