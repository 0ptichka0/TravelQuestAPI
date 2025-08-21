using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQ.UseCases.TravelQuest.Commands.Delete
{
    public class DeleteUserCommand : IRequest<Result>
    {
        public DeleteUserCommand(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
