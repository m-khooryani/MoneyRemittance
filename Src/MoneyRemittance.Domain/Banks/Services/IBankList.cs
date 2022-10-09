namespace MoneyRemittance.Domain.Banks.Services;

public interface IBankList
{
    Task<BankDto[]> GetBanksAsync(string country);
}

public record BankDto(string Name, string Code);
