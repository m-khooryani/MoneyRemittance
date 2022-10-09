using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace MoneyRemittance.Infrastructure.Configuration.B2BApi;

public class B2BApiModule : Module
{
    private readonly string _endpoint;

    public B2BApiModule(string endpoint)
    {
        _endpoint = endpoint;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var b2bApiConfig = new B2BApiConfig(_endpoint);
        builder.RegisterInstance(b2bApiConfig)
            .AsSelf()
            .SingleInstance();
        builder.RegisterType<B2BApiClient>()
            .AsSelf()
            .SingleInstance();

        builder.Register(_ =>
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<IHttpClientFactory>();
        });
    }
}
