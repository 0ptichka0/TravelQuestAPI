using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateCPRunHandler : IRequestHandler<UpdateCPRunCommand, Result<CPRunDTO>>
    {
        private readonly ICPRunService _service;
        private readonly IMapper _mapper;
        public UpdateCPRunHandler(ICPRunService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<CPRunDTO>> Handle(UpdateCPRunCommand request, CancellationToken cancellationToken)
        {
            var data = await _service.GetAsync(new IntId(request.Id));

            if (data == null) return Result.NotFound();

            data.UpdateRunId(new RunId(request.Model.RunId));
            data.UpdateCPId(new CPId(request.Model.CPId));
            data.UpdateScores(request.Model.Scores);

            var res = await _service.UpdateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<CPRunDTO>(data);
            return Result.Success(dto);

        }
    }
}
