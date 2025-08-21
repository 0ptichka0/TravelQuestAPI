using Ardalis.Result;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteTeamResultHandler : IRequestHandler<DeleteTeamResultCommand, Result>
    {
        private readonly ITeamService _service;
        public DeleteTeamResultHandler(ITeamService service)
        {
            _service = service;
        }
        public async Task<Result> Handle(DeleteTeamResultCommand request, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(new IntId(request.Id));
        }
    }
}
