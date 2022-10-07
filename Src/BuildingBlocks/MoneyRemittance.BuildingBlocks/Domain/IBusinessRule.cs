namespace MoneyRemittance.BuildingBlocks.Domain;

public interface IBusinessRule
{
    Task<bool> IsViolatedAsync();
    string Description { get; }
}
