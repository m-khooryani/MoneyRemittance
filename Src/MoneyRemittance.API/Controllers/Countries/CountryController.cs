using Microsoft.AspNetCore.Mvc;
using MoneyRemittance.API.Configuration.Authorization;
using MoneyRemittance.Application.Countries.Get;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Countries.Services;

namespace MoneyRemittance.API.Controllers.Countries;

[Route("api/countries")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly IServiceMediator _mediator;

    public CountryController(IServiceMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [RequirePermission("Get-country-list")]
    [ProducesResponseType(typeof(CountryDto[]), statusCode: 200)]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await _mediator.CommandAsync(
            new GetCountriesCommand());

        return Ok(countries);
    }
}
