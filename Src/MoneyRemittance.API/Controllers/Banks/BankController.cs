using Microsoft.AspNetCore.Mvc;
using MoneyRemittance.API.Configuration.Authorization;
using MoneyRemittance.Application.Banks.GetList;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Banks.Services;

namespace MoneyRemittance.API.Controllers.Banks;

[Route("api/banks")]
[ApiController]
public class BankController : ControllerBase
{
    private readonly IServiceMediator _mediator;

    public BankController(IServiceMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [RequirePermission("Get-banks-list")]
    [ProducesResponseType(typeof(BankDto[]), statusCode: 200)]
    public async Task<IActionResult> GetBankList(
        [FromQuery] string country)
    {
        var banks = await _mediator.CommandAsync(
            new GetBankListCommand(country));

        return Ok(banks);
    }
}
