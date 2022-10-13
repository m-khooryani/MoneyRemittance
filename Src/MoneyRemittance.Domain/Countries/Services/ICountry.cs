namespace MoneyRemittance.Domain.Countries.Services;

public interface ICountry
{
    Task<CountryDto[]> GetCountriesAsync();
}

public record CountryDto(string Name, string Code);
