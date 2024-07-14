namespace TheOne.Extensions.DependencyInjection.Microsoft.Tests.Artifacts;

public interface ITransientService
{
}

public interface IKeyedTransientService
{
}

[Component(Scope = ComponentLifetime.Transient)]
public class TransientService : ITransientService
{
}

[Component(Scope = ComponentLifetime.Transient, Name = "KeyedTransientService1", Priority = 100)]
public class KeyedTransientService1 : IKeyedTransientService
{
}

[Component(Scope = ComponentLifetime.Transient, Name = "KeyedTransientService2")]
public class KeyedTransientService2 : IKeyedTransientService
{
}

[Component(Scope = ComponentLifetime.Transient)]
public class KeyedEmptyTransientService : IKeyedTransientService
{
}