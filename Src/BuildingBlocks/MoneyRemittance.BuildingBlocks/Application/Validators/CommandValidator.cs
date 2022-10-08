using FluentValidation;
using MoneyRemittance.BuildingBlocks.Application.Contracts;

namespace MoneyRemittance.BuildingBlocks.Application.Validators;

public abstract class CommandValidator<T> : AbstractValidator<T>
    where T : ICommandBase
{
}
