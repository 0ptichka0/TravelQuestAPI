using MediatR;
using Microsoft.AspNetCore.Mvc;
using TQ.UseCases.TravelQuest.Commands.Create;
using TQ.UseCases.TravelQuest.Commands.Delete;
using TQ.UseCases.TravelQuest.DTOs;
using TQ.UseCases.TravelQuest.Queries.User.Get;
using TQ.UseCases.TravelQuest.Queries.User.List;
using TQ.UseCases.TrevalQuest.Commands.Update;

namespace TQ.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetUserListQuery());
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetUserQuery(id));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDTO data)
        {
            var result = await _mediator.Send(new CreateUserCommand(data));
            return result.IsSuccess ?
                Created($"api/UserController/{result.Value.Id}", result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDTO data)
        {
            var result = await _mediator.Send(new UpdateUserCommand(id, data));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            if (result.IsSuccess)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
