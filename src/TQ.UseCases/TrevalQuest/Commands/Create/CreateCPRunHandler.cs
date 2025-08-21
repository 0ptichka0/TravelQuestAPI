using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateCPRunHandler : IRequestHandler<CreateCPRunCommand, Result<CPRunDTO>>
    {
        private readonly ICPRunService _service;
        private readonly IMapper _mapper;
        public CreateCPRunHandler(ICPRunService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<CPRunDTO>> Handle(CreateCPRunCommand request, CancellationToken cancellationToken)
        {

            var data = new CPRun(new RunId(request.Model.RunId), new CPId(request.Model.CPId), request.Model.Scores);

            var res = await _service.CreateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<CPRunDTO>(res.Value);
            return dto;
        }
    }
}
