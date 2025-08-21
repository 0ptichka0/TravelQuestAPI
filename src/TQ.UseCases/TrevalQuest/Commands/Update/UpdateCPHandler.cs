using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Update
{
    public class UpdateCPHandler : IRequestHandler<UpdateCPCommand, Result<CPDTO>>
    {
        private readonly ICPService _service;
        private readonly IMapper _mapper;
        public UpdateCPHandler(ICPService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<CPDTO>> Handle(UpdateCPCommand request, CancellationToken cancellationToken)
        {
            var data = await _service.GetAsync(new CPId(request.Id));

            if (data == null) return Result.NotFound();

            data.UpdateId(new CPId(request.Model.Id));
            data.UpdateNumber(request.Model.Number);
            data.UpdateLegend(request.Model.Legend);
            data.UpdateLatitude(request.Model.Latitude);
            data.UpdateLongitude(request.Model.Longitude);
            
            var res = await _service.UpdateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<CPDTO>(data);
            return Result.Success(dto);

        }
    }
}
