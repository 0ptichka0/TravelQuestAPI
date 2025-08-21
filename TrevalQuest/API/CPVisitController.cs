using MediatR;
using Microsoft.AspNetCore.Mvc;
using TQ.UseCases.TravelQuest.Commands.Create;
using TQ.UseCases.TravelQuest.Commands.Delete;
using TQ.UseCases.TravelQuest.Commands.Update;
using TQ.UseCases.TravelQuest.DTOs;
using TQ.UseCases.TravelQuest.Queries.CPVisit.Get;
using TQ.UseCases.TravelQuest.Queries.CPVisit.List;

namespace TQ.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CPVisitController : BaseApiController
    {
        private readonly IMediator _mediator;
        public CPVisitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetCPVisitListQuery());
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetCPVisitQuery(id));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CPVisitDTO data)
        {
            var result = await _mediator.Send(new CreateCPVisitCommand(data));
            return result.IsSuccess ?
                Created($"api/CPVisitController/{result.Value.Id}", result.Value) :
                BadRequest(result.Errors);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] CPVisitDTO data)
        {
            var result = await _mediator.Send(new UpdateCPVisitCommand(id, data));
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Errors);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCPVisitCommand(id));
            if (result.IsSuccess)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
