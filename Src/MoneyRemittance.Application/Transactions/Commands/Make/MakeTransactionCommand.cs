using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Transactions;

namespace MoneyRemittance.Application.Transactions.Commands.Make;

public record MakeTransactionCommand(
    TransactionId TransactionId,
    string SenderFirstName,
    string SenderLastName,
    string SenderEmail,
    AddressInfo SenderAddress,
    DateTimeOffset DateOfBirth,
    string ToFirstName,
    string ToLastName,
    string ToCountry,
    string ToBankAccountName,
    string ToBankAccountNumber,
    string ToBankName,
    string ToBankCode,
    string FromAmount,
    decimal ExchangeRate,
    decimal Fees,
    string TransactionNumber,
    string FromCurrency) : Command<TransactionId>;
