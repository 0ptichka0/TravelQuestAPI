using Ardalis.Result;
using AutoMapper;
using MediatR;
using TQ.Core.Interfaces;
using TQ.UseCases.TravelQuest.DTOs;
using TQ.UseCases.TravelQuest.Queries.CP.List;

namespace TQ.UseCases.TravelQuest.Queries.User.List
{
    public class GetUserListHandler : IRequestHandler<GetUserListQuery, Result<IEnumerable<UserDTO>>>
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public GetUserListHandler(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<UserDTO>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var res = await _service.GetListAsync();

            if (!res.IsSuccess) return Result.NotFound();

            var list = _mapper.Map<IEnumerable<UserDTO>>(res.Value);

            return new Result<IEnumerable<UserDTO>>(list);
        }
    }
}
