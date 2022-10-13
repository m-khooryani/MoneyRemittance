using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Beneficiaries.Services;

namespace MoneyRemittance.Application.Beneficiaries.GetName;

public record GetBeneficiaryNameCommand(
    string AccountNumber,
    string BankCode) : Command<BeneficiaryNameDto>;
