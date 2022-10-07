namespace MoneyRemittance.BuildingBlocks.Domain;

public class BusinessRuleValidationException : Exception
{
    public IBusinessRule BrokenRule { get; }

    public string Details { get; }

    internal BusinessRuleValidationException(IBusinessRule brokenRule)
        : base(brokenRule.Description)
    {
        BrokenRule = brokenRule;
        Details = brokenRule.Description;
    }

    public override string ToString()
    {
        return $"{BrokenRule.GetType().FullName}: {BrokenRule.Description}";
    }
}
