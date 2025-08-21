using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.Run.List
{
    public class GetRunListHandler : IRequestHandler<GetRunListQuery, Result<IEnumerable<RunDTO>>>
    {
        private readonly IRunService _service;
        private readonly IMapper _mapper;
        public GetRunListHandler(IRunService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<RunDTO>>> Handle(GetRunListQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetListAsync();

            if (!res.IsSuccess) return Result.NotFound();

            var list = _mapper.Map<IEnumerable<RunDTO>>(res.Value);

            return new Result<IEnumerable<RunDTO>>(list);
        }
    }
}
