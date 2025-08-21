using MediatR;
using Microsoft.AspNetCore.Mvc;
using TQ.UseCases.TravelQuest.Commands.Create;
using TQ.UseCases.TravelQuest.Commands.Delete;
using TQ.UseCases.TravelQuest.Commands.Update;
using TQ.UseCases.TravelQuest.DTOs;
using TQ.UseCases.TravelQuest.Queries.Run.Get;
using TQ.UseCases.TravelQuest.Queries.Run.List;

namespace TQ.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class RunController : BaseApiController
    {
        private readonly IMediator _mediator;
        public RunController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetRunListQuery());
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _mediator.Send(new GetRunQuery(id));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RunDTO data)
        {
            var result = await _mediator.Send(new CreateRunCommand(data));
            return result.IsSuccess ?
                Created($"api/RunController/{result.Value.Id}", result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] RunDTO data)
        {
            var result = await _mediator.Send(new UpdateRunCommand(id, data));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteRunCommand(id));
            if (result.IsSuccess)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
