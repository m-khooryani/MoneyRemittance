using FluentValidation;
using MoneyRemittance.BuildingBlocks.Application.Validators;

namespace MoneyRemittance.Application.Transactions.Commands.Make;

internal class MakeTransactionCommandValidator : CommandValidator<MakeTransactionCommand>
{
    public MakeTransactionCommandValidator()
    {
        RuleFor(command => command.SenderFirstName)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.SenderLastName)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.SenderEmail)
            .EmailAddress()
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.SenderAddress.Country)
            .Length(2)
            .WithMessage("Country should be in ISO ALPHA-2 format");

        RuleFor(command => command.TransactionNumber)
            .MinimumLength(10)
            .MaximumLength(25);
    }
}
