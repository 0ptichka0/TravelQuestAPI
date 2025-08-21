using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand, Result<TeamDTO>>
    {
        private readonly ITeamService _service;
        private readonly IMapper _mapper;
        public UpdateTeamHandler(ITeamService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<TeamDTO>> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var data = await _service.GetAsync(new IntId(request.Id));

            if (data == null) return Result.NotFound();

            data.UpdateRunId(new RunId(request.Model.RunId));
            data.UpdateRegistrationDate(request.Model.RegistrationDate);
            data.UpdateName(request.Model.Name);
            data.UpdateCode(request.Model.Code);
            data.UpdateArea(request.Model.Area);
            data.UpdateGroup(request.Model.Group);

            var res = await _service.UpdateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<TeamDTO>(data);
            return Result.Success(dto);

        }
    }
}