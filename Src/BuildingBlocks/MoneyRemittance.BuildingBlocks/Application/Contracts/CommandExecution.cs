namespace MoneyRemittance.BuildingBlocks.Application.Contracts;

internal class CommandExecution
{
    private CommandExecutionStatus? _status;

    public void SetCommandExecutionStatus(CommandExecutionStatus status)
    {
        if (_status.HasValue)
        {
            throw new Exception("Already initialized");
        }
        _status = status;
    }

    public CommandExecutionStatus GetCommandExecutionStatus()
    {
        return _status ?? CommandExecutionStatus.None;
    }
}
