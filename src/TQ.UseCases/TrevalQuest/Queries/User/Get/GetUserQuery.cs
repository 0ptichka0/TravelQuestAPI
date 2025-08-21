using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Queries.User.Get
{
    public class GetUserQuery : IRequest<Result<UserDTO>>
    {
        public GetUserQuery(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
