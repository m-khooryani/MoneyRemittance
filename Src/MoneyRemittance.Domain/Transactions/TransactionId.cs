using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.Domain.Transactions;

public class TransactionId : TypedId<Guid>
{
    public TransactionId(Guid value) : base(value)
    {
    }

    public static TransactionId New()
    {
        return new TransactionId(Guid.NewGuid());
    }
}
