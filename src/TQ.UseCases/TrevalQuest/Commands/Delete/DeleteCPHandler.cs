using Ardalis.Result;
using MediatR;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Interfaces;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteCPHandler : IRequestHandler<DeleteCPCommand, Result>
    {
        private readonly ICPService _service;
        public DeleteCPHandler(ICPService service)
        {
            _service = service;
        }
        public async Task<Result> Handle(DeleteCPCommand request, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(new CPId(request.Id));
        }
    }
}
