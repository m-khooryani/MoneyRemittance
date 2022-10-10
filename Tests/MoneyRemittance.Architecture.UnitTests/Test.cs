using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace MoneyRemittance.Architecture.UnitTests;

public class Test
{
    protected static Assembly ApplicationAssembly = Assembly.Load("MoneyRemittance.Application");
    protected static Assembly DomainAssembly = Assembly.Load("MoneyRemittance.Domain");
    protected static Assembly InfrastructureAssembly = Assembly.Load("MoneyRemittance.Infrastructure");

    protected static void AssertFailingTypes(IEnumerable<Type> types)
    {
        if (types is null || !types.Any())
        {
            return;
        }
        var message = string.Join(Environment.NewLine, types
            .Select(x => x.Name));
        throw new Exception(message);
    }

    protected static void AssertArchTestResult(TestResult result)
    {
        AssertFailingTypes(result.FailingTypes);
    }

    protected static void AssertArchTestResult(ConditionList conditionList)
    {
        AssertArchTestResult(conditionList.GetResult());
    }
}
