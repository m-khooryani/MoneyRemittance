using Microsoft.AspNetCore.Mvc;
using MoneyRemittance.API.Configuration.Authorization;
using MoneyRemittance.API.Controllers.Transactions.Requests;
using MoneyRemittance.Application.Transactions.Commands.Make;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Transactions;

namespace MoneyRemittance.API.Controllers.Transactions;

[Route("api/transactions")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IServiceMediator _mediator;

    public TransactionController(IServiceMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("")]
    [RequirePermission("make-transaction")]
    [ProducesResponseType(typeof(Guid), statusCode: 200)]
    public async Task<IActionResult> MakeTransaction(
            [FromBody] MakeTransactionRequest request)
    {
        var banks = await _mediator.CommandAsync(
            new MakeTransactionCommand(
                TransactionId.New(),
                request.SenderFirstName,
                request.SenderLastName,
                request.SenderEmail,
                request.SenderAddress,
                request.DateOfBirth,
                request.ToFirstName,
                request.ToLastName,
                request.ToCountry,
                request.ToBankAccountName,
                request.ToBankAccountNumber,
                request.ToBankName,
                request.ToBankCode,
                request.FromAmount,
                request.ExchangeRate,
                request.Fees,
                request.TransactionNumber,
                request.FromCurrency));

        return Ok(banks);
    }
}
