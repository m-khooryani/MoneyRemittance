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

    [HttpPost("get-transaction-status")]
    public IActionResult GetTransactionStatus([FromBody] TransactionStatusRequest request)
    {
        var status = Enum.GetValues(typeof(TransactionStatus))          
            .OfType<TransactionStatus>()              
            .OrderBy(e => Guid.NewGuid()) 
            .First();
        return Ok(new TransactionStatusResponse(request.TransactionId, status));
    }
}
