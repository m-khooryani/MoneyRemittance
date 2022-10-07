using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.Domain.Transactions.Events;

public record TransactionMadeDomainEvent(
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
    string FromCurrency) : DomainEvent(TransactionId);
