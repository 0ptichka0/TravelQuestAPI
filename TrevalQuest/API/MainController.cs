using MediatR;
using Microsoft.AspNetCore.Mvc;
using TQ.UseCases.TrevalQuest.Queries.Main.Add;
using TQ.UseCases.TrevalQuest.Queries.Main.Patten;

namespace TQ.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : BaseApiController
    {
        private readonly IMediator _mediator;
        public MainController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("pattern-new-run")]
        public async Task<IActionResult> ExcelPatternNewRun()
        {
            var res = await _mediator.Send(new ExcelPatternNewRunQuery());

            if (res.IsSuccess)
            {
                return File(res.Value.Item1, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{res.Value.Item2}.xlsx");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("new-run")]
        public async Task<IActionResult> ExcelNewRun(IFormFile file)
        {
            var res = await _mediator.Send(new ExcelNewRunCommand(file));

            if (res.IsSuccess)
            {
                return File(res.Value.Item1, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{res.Value.Item2}.xlsx");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("pattern-new-team")]
        public async Task<IActionResult> ExcelPatternNewTeam()
        {
            var res = await _mediator.Send(new ExcelPatternNewTeamQuery());

            if (res.IsSuccess)
            {
                return File(res.Value.Item1, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{res.Value.Item2}.xlsx");
            }
            else
            {
                return BadRequest();
            }
        }


    }
}
