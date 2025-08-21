using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateRunHandler : IRequestHandler<CreateRunCommand, Result<RunDTO>>
    {
        private readonly IRunService _service;
        private readonly IMapper _mapper;
        public CreateRunHandler(IRunService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<RunDTO>> Handle(CreateRunCommand request, CancellationToken cancellationToken)
        {
            var data = new Run(new RunId(request.Model.Id), request.Model.Name, request.Model.RunStart.ToUniversalTime(), request.Model.Duration, request.Model.Description);

            var res = await _service.CreateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<RunDTO>(res.Value);
            return dto;
        }
    }
}
