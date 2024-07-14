using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TheOne.Extensions.DependencyInjection.Core;

[Component]
public class AppContainer : IContainer
{
    public T Resolve<T>(string? name = null)
    {
        return GetContainer().Resolve<T>(name);
    }

    public IEnumerable<T> ResolveCollection<T>()
    {
        return GetContainer().ResolveCollection<T>();
    }

    private static IContainer GetContainer()
    {
        return AppContainerManager.Container ?? throw new AutoRegisterException("Container is not initialized yet");
    }
}