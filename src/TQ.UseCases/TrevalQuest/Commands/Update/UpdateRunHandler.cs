using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateRunHandler : IRequestHandler<UpdateRunCommand, Result<RunDTO>>
    {
        private readonly IRunService _service;
        private readonly IMapper _mapper;
        public UpdateRunHandler(IRunService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<RunDTO>> Handle(UpdateRunCommand request, CancellationToken cancellationToken)
        {
            var data = await _service.GetAsync(new RunId(request.Id));

            if (data == null) return Result.NotFound();

            data.UpdateName(request.Model.Name);
            data.UpdateRunStart(request.Model.RunStart);
            data.UpdateDuration(request.Model.Duration);
            data.UpdateDescription(request.Model.Description);

            var res = await _service.UpdateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<RunDTO>(data);
            return Result.Success(dto);

        }
    }
}
