using Microsoft.AspNetCore.Mvc;

namespace B2BAPI.Controllers.ExchangeRates;

[Route("")]
[ApiController]
public class ExchangeRateController : ControllerBase
{
    [HttpPost("get-exchange-rate")]
    public IActionResult GetExchangeRate([FromBody] ExchangeRateRequest _)
    {
        return Ok(new ExchangeRateRespnose(
            "Sweden",
            "Finland",
            "1.012",
            "ERT"));
    }
}
