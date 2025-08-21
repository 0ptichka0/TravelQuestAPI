using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.CPsAggregate;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateCPHandler : IRequestHandler<CreateCPCommand, Result<CPDTO>>
    {
        private readonly ICPService _service;
        private readonly IMapper _mapper;
        public CreateCPHandler(ICPService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<CPDTO>> Handle(CreateCPCommand request, CancellationToken cancellationToken)
        {

            var data = new CP(new CPId(request.Model.Id), request.Model.Number, request.Model.Legend, request.Model.Latitude, request.Model.Longitude);

            var res = await _service.CreateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<CPDTO>(res.Value);
            return dto;
        }
    }
}
