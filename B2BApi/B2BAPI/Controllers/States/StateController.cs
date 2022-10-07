using Microsoft.AspNetCore.Mvc;

namespace B2BAPI.Controllers.States;

[Route("")]
[ApiController]
public class StateController : ControllerBase
{
    [HttpPost("get-state-list")]
    public IActionResult GetStateList()
    {
        return Ok(new StateDto[]
        {
            new StateDto("CA", "California"),
            new StateDto("TX", "Texas"),
        });
    }
}
