using System.Diagnostics.CodeAnalysis;

namespace MoneyRemittance.BuildingBlocks.Extensions.Hosting;

[ExcludeFromCodeCoverage]
internal class AssemblyScanResult
{
    public Type SessionMessageProcessorType { get; }

    public AssemblyScanResult(
        Type sessionMessageProcessorType)
    {
        SessionMessageProcessorType = sessionMessageProcessorType;
    }
}
