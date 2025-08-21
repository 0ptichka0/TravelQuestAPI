using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Aggregates.UsersAggregate;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<UserDTO>>
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public CreateUserHandler(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<UserDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var data = new User(new IntId(request.Model.TeamId), request.Model.FirstName, request.Model.LastName, request.Model.Code);

            var res = await _service.CreateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<UserDTO>(res.Value);
            return dto;
        }
    }
}
