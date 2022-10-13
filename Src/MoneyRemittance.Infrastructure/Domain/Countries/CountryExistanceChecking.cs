using MoneyRemittance.Domain.Countries.Services;

namespace MoneyRemittance.Infrastructure.Domain.Countries;

internal class CountryExistanceChecking : ICountryExistanceChecking
{
    private readonly ICountry _country;

    public CountryExistanceChecking(ICountry country)
    {
        _country = country;
    }

    public async Task<bool> ExistsAsync(string country)
    {
        var countries = await _country.GetCountriesAsync();
        return countries.Select(dto => dto.Name).Contains(country);
    }
}
