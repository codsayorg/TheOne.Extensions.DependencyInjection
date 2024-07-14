namespace TheOne.Extensions.DependencyInjection.Microsoft.Tests.Artifacts;

public interface IScopedService
{
}

public interface IKeyedScopedService
{
}

[Component(Scope = ComponentLifetime.Scoped)]
public class ScopedService : IScopedService
{
}

[Component(Scope = ComponentLifetime.Scoped, Name = "KeyedScopedService1", Priority = 100)]
public class KeyedScopedService1 : IKeyedScopedService
{
}

[Component(Scope = ComponentLifetime.Scoped, Name = "KeyedScopedService2")]
public class KeyedScopedService2 : IKeyedScopedService
{
}

[Component(Scope = ComponentLifetime.Scoped)]
public class KeyedEmptyScopedService : IKeyedScopedService
{
}