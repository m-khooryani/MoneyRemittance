namespace B2BAPI.Controllers.ExchangeRates;

public record ExchangeRateRespnose(
    string SourceCountry,
    string DestinationCountry,
    string ExchangeRate,
    string ExchangeRateToken);
