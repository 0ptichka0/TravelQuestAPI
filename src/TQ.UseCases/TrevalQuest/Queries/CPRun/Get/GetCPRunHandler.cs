using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CPRun.Get
{
    public class GetCPRunHandler : IRequestHandler<GetCPRunQuery, Result<CPRunDTO>>
    {
        private readonly ICPRunService _service;
        private readonly IMapper _mapper;
        public GetCPRunHandler(ICPRunService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<CPRunDTO>> Handle(GetCPRunQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetAsync(new IntId(request.Id));

            var dto = _mapper.Map<CPRunDTO>(res);

            return Result.Success(dto);
        }
    }
}
