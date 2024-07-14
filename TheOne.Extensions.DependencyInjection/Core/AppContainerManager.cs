using System;
using System.Collections.Generic;

namespace TheOne.Extensions.DependencyInjection.Core;

public static class AppContainerManager
{
    private static IContainer? _container;
    
    /// <summary>
    /// The global container which will be resolved when initializing application.
    /// </summary>
    public static IContainer Container => _container!;

    internal static void SetContainer(IContainer container)
    {
        _container = container;
    }

    public static T Resolve<T>(string? name = null) where T : class
    {
        ArgumentNullException.ThrowIfNull(Container, nameof(Container));
        return Container.Resolve<T>(name);
    }

    public static IEnumerable<T> ResolveCollection<T>() where T : class
    {
        ArgumentNullException.ThrowIfNull(Container, nameof(Container));
        return Container.ResolveCollection<T>();
    }
}