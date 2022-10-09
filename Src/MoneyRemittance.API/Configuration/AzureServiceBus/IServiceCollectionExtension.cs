namespace Microsoft.Extensions.DependencyInjection;

internal static partial class IServiceCollectionExtension
{
    public static IServiceCollection UseProcessorBackgroundServices(
        this IServiceCollection services)
    {
        services.AddSessionMessageProcessorsFromAssembly(
            typeof(IServiceCollectionExtension).Assembly);

        return services;
    }
}
