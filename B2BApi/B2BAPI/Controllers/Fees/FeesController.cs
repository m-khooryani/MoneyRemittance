using Microsoft.AspNetCore.Mvc;

namespace B2BAPI.Controllers.Fees;

[Route("")]
[ApiController]
public class FeesController : ControllerBase
{
    [HttpPost("get-fees-list")]
    public IActionResult GetFeesList([FromBody] FeesRequest _)
    {
        return Ok(new FeesResponse(
            1.4M,
            0.1M));
    }
}
