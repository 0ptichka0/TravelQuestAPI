using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Aggregates.CPVisitsAggregate;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateCPVisitHandler : IRequestHandler<CreateCPVisitCommand, Result<CPVisitDTO>>
    {
        private readonly ICPVisitService _service;
        private readonly IMapper _mapper;
        public CreateCPVisitHandler(ICPVisitService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<CPVisitDTO>> Handle(CreateCPVisitCommand request, CancellationToken cancellationToken)
        {

            var data = new CPVisit(new IntId(request.Model.TeamId), new CPId(request.Model.CPId), request.Model.IsValid, request.Model.VisitTime);

            var res = await _service.CreateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<CPVisitDTO>(res.Value);
            return dto;
        }
    }
}
