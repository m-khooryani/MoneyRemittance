namespace B2BAPI.Controllers.Transactions;

public record AddressInfo(
    string SenderPhone,
    string SenderAddress,
    string SenderCountry,
    string SenderCity,
    string SenderPostalCode,
    string SendFromState);
