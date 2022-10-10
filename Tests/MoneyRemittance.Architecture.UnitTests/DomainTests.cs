using MoneyRemittance.BuildingBlocks.Domain;
using NetArchTest.Rules;
using Xunit;

namespace MoneyRemittance.Architecture.UnitTests;

public class DomainTests : Test
{
    [Fact]
    public void DomainEvent_Should_Have_DomainEventPostfix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEvent))
            .Or()
            .Inherit(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void BusinessRule_Should_Have_RulePostfix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(IBusinessRule))
            .Should().HaveNameEndingWith("Rule")
            .GetResult();

        AssertArchTestResult(result);
    }
}
