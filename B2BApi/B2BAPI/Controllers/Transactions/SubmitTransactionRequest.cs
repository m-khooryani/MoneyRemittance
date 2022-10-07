namespace B2BAPI.Controllers.Transactions;

public record SubmitTransactionRequest(
    string SenderFirstName,
    string SenderLastName,
    string SenderEmail,
    AddressInfo AddressInfo,
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
    string FromCurrency);
