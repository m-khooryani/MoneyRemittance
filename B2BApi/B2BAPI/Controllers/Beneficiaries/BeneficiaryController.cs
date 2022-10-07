using Microsoft.AspNetCore.Mvc;

namespace B2BAPI.Controllers.Beneficiaries;

[Route("")]
[ApiController]
public class BeneficiaryController : ControllerBase
{
    [HttpPost("get-beneficiary-name")]
    public IActionResult GetBeneficiaryName([FromBody] BeneficiaryNameRequest _)
    {
        return Ok(new
        {
            AccountName = Guid.NewGuid().ToString()[..8],
        });
    }
}
