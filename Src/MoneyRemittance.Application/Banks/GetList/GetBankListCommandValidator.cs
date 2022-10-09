using FluentValidation;

namespace MoneyRemittance.Application.Banks.GetList;

internal class GetBankListCommandValidator : AbstractValidator<GetBankListCommand>
{
    public GetBankListCommandValidator()
    {
        RuleFor(command => command.Country)
            .Length(2)
            .WithMessage("Country should be in ISO ALPHA-2 format");
    }
}
