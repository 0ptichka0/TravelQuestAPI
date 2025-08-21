using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateTeamResultHandler : IRequestHandler<UpdateTeamResultCommand, Result<TeamResultDTO>>
    {
        private readonly ITeamResultService _service;
        private readonly IMapper _mapper;
        public UpdateTeamResultHandler(ITeamResultService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<TeamResultDTO>> Handle(UpdateTeamResultCommand request, CancellationToken cancellationToken)
        {
            var data = await _service.GetAsync(new IntId(request.Id));

            if (data == null) return Result.NotFound();

            data.UpdateTeamId(new IntId(request.Model.TeamId));
            data.UpdateTotalScore(request.Model.TotalScore);
            data.UpdateElapsedTime(request.Model.ElapsedTime);
            data.UpdatePenalty(request.Model.Penalty);

            var res = await _service.UpdateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<TeamResultDTO>(data);
            return Result.Success(dto);

        }
    }
}