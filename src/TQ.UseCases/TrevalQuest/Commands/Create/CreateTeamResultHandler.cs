using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.TeamResultsAggregate;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateTeamResultHandler : IRequestHandler<CreateTeamResultCommand, Result<TeamResultDTO>>
    {
        private readonly ITeamResultService _service;
        private readonly IMapper _mapper;
        public CreateTeamResultHandler(ITeamResultService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<TeamResultDTO>> Handle(CreateTeamResultCommand request, CancellationToken cancellationToken)
        {
            var data = new TeamResult(new IntId(request.Model.TeamId), request.Model.TotalScore, request.Model. ElapsedTime, request.Model.Penalty);

            var res = await _service.CreateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<TeamResultDTO>(res.Value);
            return dto;
        }
    }
}
