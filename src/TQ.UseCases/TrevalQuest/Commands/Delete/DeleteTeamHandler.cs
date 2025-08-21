using Ardalis.Result;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamCommand, Result>
    {
        private readonly ITeamService _service;
        public DeleteTeamHandler(ITeamService service)
        {
            _service = service;
        }
        public async Task<Result> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(new IntId(request.Id));
        }
    }
}
