using MoneyRemittance.Domain.States.Services;
using Swashbuckle.AspNetCore.Filters;

namespace MoneyRemittance.API.Controllers.States.Examples;

internal class StateDtoExample : IExamplesProvider<StateDto[]>
{
    public StateDto[] GetExamples()
    {
        return new StateDto[]
        {
            new StateDto("CA", "California"),
            new StateDto("TX", "Texas")
        };
    }
}
