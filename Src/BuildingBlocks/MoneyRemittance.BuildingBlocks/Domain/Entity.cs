namespace MoneyRemittance.BuildingBlocks.Domain;

public abstract class Entity
{
    protected static async Task CheckRuleAsync(IBusinessRule rule)
    {
        if (await rule.IsViolatedAsync())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
}
