using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.CPVisit.Get
{
    public class GetCPVisitHandler : IRequestHandler<GetCPVisitQuery, Result<CPVisitDTO>>
    {
        private readonly ICPVisitService _service;
        private readonly IMapper _mapper;
        public GetCPVisitHandler(ICPVisitService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<CPVisitDTO>> Handle(GetCPVisitQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetAsync(new IntId(request.Id));

            var dto = _mapper.Map<CPVisitDTO>(res);

            return Result.Success(dto);
        }
    }
}
