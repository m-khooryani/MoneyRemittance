using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.States.Services;

namespace MoneyRemittance.Application.States.Get;

public record GetStatesCommand() : Command<StateDto[]>;
