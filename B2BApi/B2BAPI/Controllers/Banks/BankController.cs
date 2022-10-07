using Microsoft.AspNetCore.Mvc;

namespace B2BAPI.Controllers.Banks;

[Route("")]
[ApiController]
public class BankController : ControllerBase
{
    [HttpPost("get-bank-list")]
    public IActionResult GetBanksList([FromBody] BankListRequest _)
    {
        return Ok(new BankDto[]
        {
            new BankDto("JPMorgan Chase", "JPM"),
            new BankDto("Citigroup", "CITI"),
        });
    }
}
