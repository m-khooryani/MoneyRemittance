using Microsoft.AspNetCore.Mvc;
using MoneyRemittance.API.Configuration.Authorization;
using MoneyRemittance.Application.States.Get;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.States.Services;

namespace MoneyRemittance.API.Controllers.States;

[Route("api/states")]
[ApiController]
public class StateController : ControllerBase
{
    private readonly IServiceMediator _mediator;

    public StateController(IServiceMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [RequirePermission("Get-state-list")]
    [ProducesResponseType(typeof(StateDto[]), statusCode: 200)]
    public async Task<IActionResult> GetStates()
    {
        var states = await _mediator.CommandAsync(
            new GetStatesCommand());

        return Ok(states);
    }
}
