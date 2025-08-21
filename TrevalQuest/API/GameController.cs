using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TQ.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : BaseApiController
    {
        private readonly IMediator _mediator;
        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    var result = await _mediator.Send(new GetGameInfoQuery(id));
        //    return result.IsSuccess ?
        //        Ok(result.Value) :
        //        BadRequest(result.Errors);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Post(GameInfoDTO data)
        //{
        //    var result = await _mediator.Send(new CreateGameInfoCommand(data));
        //    return result.IsSuccess ?
        //        Created($"api/GameController/{result.Value.Id}", result.Value) :
        //        BadRequest(result.Errors);
        //}
    }
}
