using System.Diagnostics.CodeAnalysis;
using MoneyRemittance.BuildingBlocks.Domain;
using Xunit;

namespace MoneyRemittance.Domain.UnitTests._SeedWork;

[ExcludeFromCodeCoverage]
public abstract class Test
{
    public static T AssertPublishedDomainEvent<T>(AggregateRoot aggregate)
        where T : IDomainEvent
    {
        var domainEvent = aggregate.DomainEvents
            .OfType<T>()
            .SingleOrDefault();

        if (domainEvent is null)
        {
            throw new Exception($"{typeof(T).Name} is not published.");
        }

        return domainEvent;
    }

    public static void AssertViolatedRule<TRule>(Action testDelegate)
        where TRule : class, IBusinessRule
    {
        var businessRuleValidationException = Assert.Throws<BusinessRuleValidationException>(testDelegate);
        if (businessRuleValidationException != null)
        {
            Assert.IsType<TRule>(businessRuleValidationException.BrokenRule);
        }
    }

    public static async Task AssertViolatedRuleAsync<TRule>(Func<Task> testDelegate)
        where TRule : class, IBusinessRule
    {
        var businessRuleValidationException = await Assert.ThrowsAsync<BusinessRuleValidationException>(testDelegate);
        if (businessRuleValidationException != null)
        {
            Assert.IsType<TRule>(businessRuleValidationException.BrokenRule);
        }
    }
}
