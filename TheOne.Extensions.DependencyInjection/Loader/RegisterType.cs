using System;

namespace TheOne.Extensions.DependencyInjection.Loader;

public record InjectableMetadata(Type Type, InjectableAttribute Attr);

public record RegisterType(InjectableMetadata Info, Type Implementation)
{
}