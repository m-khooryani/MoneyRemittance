namespace MoneyRemittance.Domain.ExchangeRates.Services;

public interface IExchangeRate
{
    Task<ExchangeRateDto> GetExchangeRateAsync(string from, string to);
}

public record ExchangeRateDto(
    string SourceCountry,
    string DestinationCountry,
    string ExchangeRate,
    string ExchangeRateToken);