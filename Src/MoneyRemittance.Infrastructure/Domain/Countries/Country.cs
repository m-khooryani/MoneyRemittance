using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Infrastructure.Configuration.B2BApi;

namespace MoneyRemittance.Infrastructure.Domain.Countries;

internal class Country : ICountry
{
    private readonly B2BApiClient _apiClient;

    public Country(B2BApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<CountryDto[]> GetCountriesAsync()
    {
        return await _apiClient.GetCountryList();
    }
}
