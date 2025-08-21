using Ardalis.Result;
using MediatR;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Interfaces;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteRunHandler : IRequestHandler<DeleteRunCommand, Result>
    {
        private readonly IRunService _service;
        public DeleteRunHandler(IRunService service)
        {
            _service = service;
        }
        public async Task<Result> Handle(DeleteRunCommand request, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(new RunId(request.Id));
        }
    }
}
