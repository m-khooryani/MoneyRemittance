using Microsoft.AspNetCore.Mvc;

namespace B2BAPI.Controllers.Transactions;

[Route("")]
[ApiController]
public class TransactionController : ControllerBase
{
    [HttpPost("submit-transaction")]
    public IActionResult SubmitTransaction([FromBody] SubmitTransactionRequest _)
    {
        return Ok(new
        {
            transactionId = Guid.NewGuid().ToString()
        });
    }
}
