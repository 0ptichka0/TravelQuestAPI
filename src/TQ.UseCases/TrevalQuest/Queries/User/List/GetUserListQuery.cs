using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.User.List
{
    public class GetUserListQuery : IRequest<Result<IEnumerable<UserDTO>>>
    {
        public GetUserListQuery()
        {

        }
    }
}
