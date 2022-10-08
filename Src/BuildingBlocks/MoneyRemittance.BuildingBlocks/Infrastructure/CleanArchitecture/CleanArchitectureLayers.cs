using System.Reflection;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.CleanArchitecture;

public class CleanArchitectureLayers
{
    public Assembly DomainLayer { get; init; }
    public Assembly ApplicationLayer { get; init; }
}
