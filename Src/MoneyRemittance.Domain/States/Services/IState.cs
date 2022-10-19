namespace MoneyRemittance.Domain.States.Services;

public interface IState
{
    Task<StateDto[]> GetStatesAsync();
}

public record StateDto(string Code, string Name);
