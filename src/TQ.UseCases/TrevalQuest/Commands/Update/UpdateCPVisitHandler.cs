using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateCPVisitHandler : IRequestHandler<UpdateCPVisitCommand, Result<CPVisitDTO>>
    {
        private readonly ICPVisitService _service;
        private readonly IMapper _mapper;
        public UpdateCPVisitHandler(ICPVisitService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<CPVisitDTO>> Handle(UpdateCPVisitCommand request, CancellationToken cancellationToken)
        {
            var data = await _service.GetAsync(new IntId(request.Id));

            if (data == null) return Result.NotFound();

            data.UpdateTeamId(new IntId(request.Model.TeamId));
            data.UpdateCPId(new CPId(request.Model.CPId));
            data.UpdateIsValid(request.Model.IsValid);
            data.UpdateVisitTime(request.Model.VisitTime);

            var res = await _service.UpdateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<CPVisitDTO>(data);
            return Result.Success(dto);

        }
    }
}