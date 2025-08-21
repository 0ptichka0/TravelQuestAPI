using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CP.Get
{
    public class GetCPHandler : IRequestHandler<GetCPQuery, Result<CPDTO>>
    {
        private readonly ICPService _service;
        private readonly IMapper _mapper;
        public GetCPHandler(ICPService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<CPDTO>> Handle(GetCPQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetAsync(new CPId(request.Id));

            var dto = _mapper.Map<CPDTO>(res);

            return Result.Success(dto);
        }
    }
}
