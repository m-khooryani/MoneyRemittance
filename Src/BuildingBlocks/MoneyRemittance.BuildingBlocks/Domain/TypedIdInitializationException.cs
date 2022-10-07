namespace MoneyRemittance.BuildingBlocks.Domain;

public class TypedIdInitializationException : Exception
{
    public TypedIdInitializationException(string message)
        : base(message)
    {
    }
}
