using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Application.Validators;
using NetArchTest.Rules;
using Xunit;

namespace MoneyRemittance.Architecture.UnitTests;

public class ApplicationTests : Test
{
    [Fact]
    public void Commands_should_have_command_postfix()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(Command<>)).Or()
            .Inherit(typeof(Command))
            .Should()
            .HaveNameEndingWith("Command");

        AssertArchTestResult(result);
    }

    [Fact]
    public void Command_validators_should_not_be_public()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(CommandValidator<>))
            .Should()
            .NotBePublic();

        AssertArchTestResult(result);
    }
}
