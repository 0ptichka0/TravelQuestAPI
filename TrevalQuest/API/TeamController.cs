using MediatR;
using Microsoft.AspNetCore.Mvc;
using TQ.UseCases.TravelQuest.Commands.Create;
using TQ.UseCases.TravelQuest.Commands.Delete;
using TQ.UseCases.TravelQuest.Commands.Update;
using TQ.UseCases.TravelQuest.DTOs;
using TQ.UseCases.TravelQuest.Queries.Team.Get;
using TQ.UseCases.TravelQuest.Queries.Team.List;

namespace TQ.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : BaseApiController
    {
        private readonly IMediator _mediator;
        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetTeamListQuery());
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetTeamQuery(id));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TeamDTO data)
        {
            var result = await _mediator.Send(new CreateTeamCommand(data));
            return result.IsSuccess ?
                Created($"api/TeamController/{result.Value.Id}", result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] TeamDTO data)
        {
            var result = await _mediator.Send(new UpdateTeamCommand(id, data));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTeamCommand(id));
            if (result.IsSuccess)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
