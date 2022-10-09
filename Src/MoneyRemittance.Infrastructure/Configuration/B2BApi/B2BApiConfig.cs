namespace MoneyRemittance.Infrastructure.Configuration.B2BApi;

internal class B2BApiConfig
{
    public string Endpoint { get; init; }

    public B2BApiConfig(string endpoint)
    {
        Endpoint = endpoint;
    }
}
