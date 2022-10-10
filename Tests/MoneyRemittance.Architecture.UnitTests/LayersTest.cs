using NetArchTest.Rules;
using Xunit;

namespace MoneyRemittance.Architecture.UnitTests;

public class LayersTest : Test
{
    [Fact]
    public void Application_should_not_have_dependency_on_Infrastructure()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void Domain_should_not_have_dependency_on_Application()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void Domain_should_not_have_dependency_on_Infrastructure()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        AssertArchTestResult(result);
    }
}
