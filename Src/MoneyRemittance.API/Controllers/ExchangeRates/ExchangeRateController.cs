using Microsoft.AspNetCore.Mvc;
using MoneyRemittance.API.Configuration.Authorization;
using MoneyRemittance.Application.ExchangeRates.Get;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.ExchangeRates.Services;

namespace MoneyRemittance.API.Controllers.ExchangeRates;

[Route("api/exchange-rates")]
[ApiController]
public class ExchangeRateController : ControllerBase
{
    private readonly IServiceMediator _mediator;

    public ExchangeRateController(IServiceMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [RequirePermission("Get-exchange-rate")]
    [ProducesResponseType(typeof(ExchangeRateDto), statusCode: 200)]
    public async Task<IActionResult> GetExchangeRate(
        [FromQuery] string from,
        [FromQuery] string to)
    {
        var exchangeRateDto = await _mediator.CommandAsync(
            new GetExchangeRateCommand(from, to));

        return Ok(exchangeRateDto);
    }
}
