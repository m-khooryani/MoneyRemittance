using MoneyRemittance.Domain.ExchangeRates.Services;
using MoneyRemittance.Infrastructure.Configuration.B2BApi;

namespace MoneyRemittance.Infrastructure.Domain.ExchangeRates;

internal class ExchangeRate : IExchangeRate
{
    private readonly B2BApiClient _apiClient;

    public ExchangeRate(B2BApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<ExchangeRateDto> GetExchangeRateAsync(string from, string to)
    {
        return await _apiClient.GetExchangeRateAsync(from, to);
    }
}
