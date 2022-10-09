using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MoneyRemittance.BuildingBlocks.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSessionMessageProcessorsFromAssembly(
        this IServiceCollection services,
        Assembly assembly)
    {
        FindSessionMessageProcessorsInAssembly(assembly.GetTypes())
            .ToList()
            .ForEach(scanResult => services.Register(scanResult));
        services.AddSingleton<IJsonMessageResolver, PlainMessageJsonResolver>();

        return services;
    }

    private static IEnumerable<AssemblyScanResult> FindSessionMessageProcessorsInAssembly(IEnumerable<Type> types)
    {
        return
            from type
            in types
            where !type.IsAbstract &&
                !type.IsGenericTypeDefinition &&
                type.BaseType != null &&
                type.BaseType.BaseType != null &&
                type.BaseType.BaseType.IsGenericType &&
                type.BaseType.BaseType.GetGenericTypeDefinition() == typeof(SessionMessageHandler<>)
            select new AssemblyScanResult(type);
    }

    private static IServiceCollection Register(
       this IServiceCollection services,
       AssemblyScanResult scanResult)
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton(
            typeof(IHostedService), scanResult.SessionMessageProcessorType));

        return services;
    }
}

