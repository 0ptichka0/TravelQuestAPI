using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CPRun.List
{
    public class GetCPRunListHandler : IRequestHandler<GetCPRunListQuery, Result<IEnumerable<CPRunDTO>>>
    {
        private readonly ICPRunService _service;
        private readonly IMapper _mapper;
        public GetCPRunListHandler(ICPRunService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<CPRunDTO>>> Handle(GetCPRunListQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetListAsync();

            if (!res.IsSuccess) return Result.NotFound();

            var list = _mapper.Map<IEnumerable<CPRunDTO>>(res.Value);

            return new Result<IEnumerable<CPRunDTO>>(list);
        }
    }
}
