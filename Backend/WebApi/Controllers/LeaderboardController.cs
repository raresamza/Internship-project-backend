using Backend.Application.Leaderboard.Queries;
using Backend.Application.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LeaderboardController:ControllerBase
{


    private readonly IMediator _mediator;

    public LeaderboardController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("class")]
    public async Task<IActionResult> GetLeaderboard()
    {
        var result = await _mediator.Send(new GetLeaderboard());
        return Ok(result);
    }
}
