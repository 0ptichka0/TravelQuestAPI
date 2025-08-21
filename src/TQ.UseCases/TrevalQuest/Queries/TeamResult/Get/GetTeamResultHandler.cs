using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.TeamResult.Get
{
    public class GetTeamResultHandler : IRequestHandler<GetTeamResultQuery, Result<TeamResultDTO>>
    {
        private readonly ITeamResultService _service;
        private readonly IMapper _mapper;
        public GetTeamResultHandler(ITeamResultService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<TeamResultDTO>> Handle(GetTeamResultQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetAsync(new IntId(request.Id));

            var dto = _mapper.Map<TeamResultDTO>(res);

            return Result.Success(dto);
        }
    }
}
