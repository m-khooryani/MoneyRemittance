using Microsoft.AspNetCore.Mvc;
using MoneyRemittance.API.Configuration.Authorization;
using MoneyRemittance.Application.Beneficiaries.GetName;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Beneficiaries.Services;

namespace MoneyRemittance.API.Controllers.Beneficiaries;

[Route("api/beneficiaries")]
[ApiController]
public class BeneficiaryController : ControllerBase
{
    private readonly IServiceMediator _mediator;

    public BeneficiaryController(IServiceMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [RequirePermission("Get-beneficiary-name")]
    [ProducesResponseType(typeof(BeneficiaryNameDto), statusCode: 200)]
    public async Task<IActionResult> GetBeneficiaryName(
        [FromQuery] string accountNumber,
        [FromQuery] string bankCode)
    {
        var beneficiaryNameDto = await _mediator.CommandAsync(
            new GetBeneficiaryNameCommand(accountNumber, bankCode));

        return Ok(beneficiaryNameDto);
    }
}
