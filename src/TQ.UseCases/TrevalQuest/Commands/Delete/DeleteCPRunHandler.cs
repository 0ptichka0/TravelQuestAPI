using Ardalis.Result;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteCPRunHandler : IRequestHandler<DeleteCPRunCommand, Result>
    {
        private readonly ICPRunService _service;
        public DeleteCPRunHandler(ICPRunService service)
        {
            _service = service;
        }
        public async Task<Result> Handle(DeleteCPRunCommand request, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(new IntId(request.Id));
        }
    }
}
