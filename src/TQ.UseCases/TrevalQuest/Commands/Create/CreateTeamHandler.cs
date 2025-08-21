using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Aggregates.TeamsAggregate;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, Result<TeamDTO>>
    {
        private readonly ITeamService _service;
        private readonly IMapper _mapper;
        public CreateTeamHandler(ITeamService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<TeamDTO>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {

            var data = new Team(new RunId(request.Model.RunId), request.Model.RegistrationDate, request.Model.Name, request.Model.Code, request.Model.Area, request.Model.Group);

            var res = await _service.CreateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<TeamDTO>(res.Value);
            return dto;
        }
    }
}
