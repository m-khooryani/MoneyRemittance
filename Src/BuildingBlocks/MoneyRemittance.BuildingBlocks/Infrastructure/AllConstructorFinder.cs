using System.Collections.Concurrent;
using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace MoneyRemittance.BuildingBlocks.Infrastructure;

public class AllConstructorFinder : IConstructorFinder
{
    private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> _cache =
        new();

    public ConstructorInfo[] FindConstructors(Type targetType)
    {
        var constructors = _cache.GetOrAdd(targetType,
            t => t.GetTypeInfo().DeclaredConstructors.ToArray());

        return constructors.Length > 0 ? constructors : throw new NoConstructorsFoundException(targetType);
    }
}
