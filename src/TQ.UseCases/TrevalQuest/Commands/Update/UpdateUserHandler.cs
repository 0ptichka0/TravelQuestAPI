using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.Commands.Update;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TrevalQuest.Commands.Update
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDTO>>
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public UpdateUserHandler(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var data = await _service.GetAsync(new IntId(request.Id));

            if (data == null) return Result.NotFound();

            data.UpdateTeamId(new IntId(request.Model.TeamId));
            data.UpdateFirstName(request.Model.FirstName);
            data.UpdateLastName(request.Model.LastName);
            data.UpdateCode(request.Model.Code);

            var res = await _service.UpdateAsync(data);
            if (!res.IsSuccess) return Result.Error(res.Errors.ToString());
            var dto = _mapper.Map<UserDTO>(data);
            return Result.Success(dto);

        }
    }
}