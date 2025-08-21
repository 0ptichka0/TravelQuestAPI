using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.Team.List
{
    public class GetTeamListHandler : IRequestHandler<GetTeamListQuery, Result<IEnumerable<TeamDTO>>>
    {
        private readonly ITeamService _service;
        private readonly IMapper _mapper;
        public GetTeamListHandler(ITeamService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<TeamDTO>>> Handle(GetTeamListQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetListAsync();

            if (!res.IsSuccess) return Result.NotFound();

            var list = _mapper.Map<IEnumerable<TeamDTO>>(res.Value);

            return new Result<IEnumerable<TeamDTO>>(list);
        }
    }
}
