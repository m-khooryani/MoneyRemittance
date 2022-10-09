using MoneyRemittance.Domain.Transactions;

namespace MoneyRemittance.API.Controllers.Transactions.Requests;

public class MakeTransactionRequest
{
    public string SenderFirstName { get; init; }
    public string SenderLastName { get; init; }
    public string SenderEmail { get; init; }
    public AddressInfo SenderAddress { get; init; }
    public DateTimeOffset DateOfBirth { get; init; }
    public string ToFirstName { get; init; }
    public string ToLastName { get; init; }
    public string ToCountry { get; init; }
    public string ToBankAccountName { get; init; }
    public string ToBankAccountNumber { get; init; }
    public string ToBankName { get; init; }
    public string ToBankCode { get; init; }
    public string FromAmount { get; init; }
    public decimal ExchangeRate { get; init; }
    public decimal Fees { get; init; }
    public string TransactionNumber { get; init; }
    public string FromCurrency { get; init; }
}
