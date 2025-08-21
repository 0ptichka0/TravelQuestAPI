using MediatR;
using Microsoft.AspNetCore.Mvc;
using TQ.UseCases.TravelQuest.Commands.Create;
using TQ.UseCases.TravelQuest.Commands.Delete;
using TQ.UseCases.TravelQuest.Commands.Update;
using TQ.UseCases.TravelQuest.DTOs;
using TQ.UseCases.TravelQuest.Queries.CPRun.Get;
using TQ.UseCases.TravelQuest.Queries.CPRun.List;

namespace TQ.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CPRunController : BaseApiController
    {
        private readonly IMediator _mediator;
        public CPRunController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetCPRunListQuery());
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetCPRunQuery(id));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CPRunDTO data)
        {
            var result = await _mediator.Send(new CreateCPRunCommand(data));
            return result.IsSuccess ?
                Created($"api/CPController/{result.Value.Id}", result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] CPRunDTO data)
        {
            var result = await _mediator.Send(new UpdateCPRunCommand(id, data));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCPRunCommand(id));
            if (result.IsSuccess)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
