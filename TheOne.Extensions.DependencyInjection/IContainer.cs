using System;
using System.Collections.Generic;

namespace TheOne.Extensions.DependencyInjection;

public interface IContainer
{
    T Resolve<T>(string? name = null);

    IEnumerable<T> ResolveCollection<T>();
}