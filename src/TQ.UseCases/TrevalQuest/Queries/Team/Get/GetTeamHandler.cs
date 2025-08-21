using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.Team.Get
{
    public class GetTeamHandler : IRequestHandler<GetTeamQuery, Result<TeamDTO>>
    {
        private readonly ITeamService _service;
        private readonly IMapper _mapper;
        public GetTeamHandler(ITeamService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<TeamDTO>> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetAsync(new IntId(request.Id));

            var dto = _mapper.Map<TeamDTO>(res);

            return Result.Success(dto);
        }
    }
}
