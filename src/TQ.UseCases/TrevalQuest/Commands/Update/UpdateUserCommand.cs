using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TrevalQuest.Commands.Update
{
    public class UpdateUserCommand : IRequest<Result<UserDTO>>
    {
        public UpdateUserCommand(int id, UserDTO model)
        {
            Id = id;
            Model = model;
        }
        public int Id { get; private set; }
        public UserDTO Model { get; private set; }
    }
}
