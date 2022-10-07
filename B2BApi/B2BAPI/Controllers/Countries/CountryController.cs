using Microsoft.AspNetCore.Mvc;

namespace B2BAPI.Controllers.Countries;

//[Route("api/countries")]
[ApiController]
public class CountryController : ControllerBase
{
    [HttpPost("get-country-list")]
    [ProducesResponseType(typeof(void), statusCode: 200)]
    public IActionResult GetCountries()
    {
        return Ok("success");
    }
}
