using Ardalis.Result;
using MediatR;
using TQ.UseCases.TravelQuest.DTOs;

namespace TQ.UseCases.TravelQuest.Commands.Create
{
    public class CreateUserCommand : IRequest<Result<UserDTO>>
    {
        public CreateUserCommand(UserDTO model)
        {
            Model = model;
        }
        public UserDTO Model { get; set; }
    }
}
