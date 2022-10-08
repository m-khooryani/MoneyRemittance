namespace MoneyRemittance.BuildingBlocks.Application.Contracts;

internal enum CommandExecutionStatus
{
    None = 0,
    Retry = 1,
    LastRetry = 2
}
