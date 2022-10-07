using Microsoft.AspNetCore.Mvc;

namespace B2BAPI.Controllers.Countries;

[Route("")]
[ApiController]
public class CountryController : ControllerBase
{
    [HttpPost("get-country-list")]
    public IActionResult GetCountries()
    {
        return Ok(new CountryDto[]
        {
            new CountryDto("Iran", "IR"),
            new CountryDto("Sweden", "SE"),
        });
    }
}
