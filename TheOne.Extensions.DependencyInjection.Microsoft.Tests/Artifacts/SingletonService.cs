namespace TheOne.Extensions.DependencyInjection.Microsoft.Tests.Artifacts;

public interface ISingletonService
{
}

[Component]
public class SingletonService : ISingletonService
{
}

public interface IKeyedSingletonService
{
}

[Component(Name = "KeyedSingletonService1", Priority = 100)]
public class KeyedSingletonService1 : IKeyedSingletonService
{
}

[Component(Name = "KeyedSingletonService2")]
public class KeyedSingletonService2 : IKeyedSingletonService
{
}

[Component]
public class KeyedEmptySingletonService : IKeyedSingletonService
{
}