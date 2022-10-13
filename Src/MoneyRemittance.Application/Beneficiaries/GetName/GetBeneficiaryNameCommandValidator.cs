using FluentValidation;
using MoneyRemittance.BuildingBlocks.Application.Validators;

namespace MoneyRemittance.Application.Beneficiaries.GetName;

internal class GetBeneficiaryNameCommandValidator : CommandValidator<GetBeneficiaryNameCommand>
{
    public GetBeneficiaryNameCommandValidator()
    {
        RuleFor(command => command.AccountNumber)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.BankCode)
            .NotNull()
            .NotEmpty();
    }
}
