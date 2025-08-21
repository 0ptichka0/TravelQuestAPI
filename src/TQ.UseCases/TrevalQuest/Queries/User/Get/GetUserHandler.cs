using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.User.Get
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, Result<UserDTO>>
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public GetUserHandler(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<UserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetAsync(new IntId(request.Id));

            var dto = _mapper.Map<UserDTO>(res);

            return Result.Success(dto);
        }
    }
}
