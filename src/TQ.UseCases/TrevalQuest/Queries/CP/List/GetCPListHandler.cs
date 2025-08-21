using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CP.List
{
    public class GetCPListHandler : IRequestHandler<GetCPListQuery, Result<IEnumerable<CPDTO>>>
    {
        private readonly ICPService _service;
        private readonly IMapper _mapper;
        public GetCPListHandler(ICPService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<CPDTO>>> Handle(GetCPListQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetListAsync();

            if (!res.IsSuccess) return Result.NotFound();

            var list = _mapper.Map<IEnumerable<CPDTO>>(res.Value);

            return new Result<IEnumerable<CPDTO>>(list);
        }
    }
}
