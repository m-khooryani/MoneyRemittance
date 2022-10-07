namespace MoneyRemittance.Domain.Countries.Services;

public interface ICountryExistanceChecking
{
    Task<bool> ExistsAsync(string country);
}
