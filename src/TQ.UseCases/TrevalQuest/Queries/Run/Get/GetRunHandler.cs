using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.Run.Get
{
    public class GetRunHandler : IRequestHandler<GetRunQuery, Result<RunDTO>>
    {
        private readonly IRunService _service;
        private readonly IMapper _mapper;
        public GetRunHandler(IRunService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<RunDTO>> Handle(GetRunQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetAsync(new RunId(request.Id));

            var dto = _mapper.Map<RunDTO>(res);

            return Result.Success(dto);
        }
    }
}
