namespace MoneyRemittance.Domain.Beneficiaries.Services;

public interface IBeneficiary
{
    Task<BeneficiaryNameDto> GetNameAsync(string accountNumber, string bankCode);
}

public record BeneficiaryNameDto(string AccountName);
