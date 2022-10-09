using Swashbuckle.AspNetCore.Filters;

namespace MoneyRemittance.API.Controllers.FakeAuthentications.Examples;

internal class FakeAuthenticationResponseExample : IExamplesProvider<FakeAuthenticationResponse>
{
    public FakeAuthenticationResponse GetExamples()
    {
        return new FakeAuthenticationResponse
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiYTQzMWQ5NWYtNzVmMi00YWZkLTkxYzItZGRhZDM5ZjQwODExIiwibmJmIjoxNjQ5MTg5MTY3LCJleHAiOjE5NjQ4MDgzNjcsImlhdCI6MTY0OTE4OTE2N30.CndaVZWRGF6Pyc3xZMcEwNvGZJ69Kc2KzUXYK8qpIRE"
        };
    }
}
