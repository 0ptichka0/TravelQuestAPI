using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CPVisit.List
{
    public class GetCPVisitListHandler : IRequestHandler<GetCPVisitListQuery, Result<IEnumerable<CPVisitDTO>>>
    {
        private readonly ICPVisitService _service;
        private readonly IMapper _mapper;
        public GetCPVisitListHandler(ICPVisitService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<CPVisitDTO>>> Handle(GetCPVisitListQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetListAsync();

            if (!res.IsSuccess) return Result.NotFound();

            var list = _mapper.Map<IEnumerable<CPVisitDTO>>(res.Value);

            return new Result<IEnumerable<CPVisitDTO>>(list);
        }
    }
}
