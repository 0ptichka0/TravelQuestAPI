using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.TeamResult.List
{
    public class GetTeamResultListHandler : IRequestHandler<GetTeamResultListQuery, Result<IEnumerable<TeamResultDTO>>>
    {
        private readonly ITeamResultService _service;
        private readonly IMapper _mapper;
        public GetTeamResultListHandler(ITeamResultService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<TeamResultDTO>>> Handle(GetTeamResultListQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetListAsync();

            if (!res.IsSuccess) return Result.NotFound();

            var list = _mapper.Map<IEnumerable<TeamResultDTO>>(res.Value);

            return new Result<IEnumerable<TeamResultDTO>>(list);
        }
    }
}
