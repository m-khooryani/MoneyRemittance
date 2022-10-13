using FluentValidation;
using MoneyRemittance.BuildingBlocks.Application.Validators;

namespace MoneyRemittance.Application.ExchangeRates.Get;

internal class GetExchangeRateCommandValidator : CommandValidator<GetExchangeRateCommand>
{
    public GetExchangeRateCommandValidator()
    {
        RuleFor(command => command.From)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.To)
            .NotNull()
            .NotEmpty();
    }
}
