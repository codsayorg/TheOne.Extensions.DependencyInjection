# TheOne.Extensions.DependencyInjection
Provide a mechanism to automatically scan and register dependencies

## Include 2 packages

### 1. TheOne.Extensions.DependencyInjection
Provide common attributes, core logics

### 2. TheOne.Extensions.DependencyInjection.Microsoft
Integrate with [Microsoft Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

## What
Provide a ready-to-use DI container, including features:
- Automatically scan assemblies for components (repositories, services, etc)
- Support multiple (named) implementations for the same type
- Flexible configure default implementation

## How to define services

```C#
public interface ILogger
{
}

@Component(Name = "console")
public class ConsoleLogger : ILogger
{
}

@Component(Name = "database")
public class DatabaseLogger : ILogger
{
}

// Default logger when asking for ILogger
@Component(Priority = 100)
public class Logger : ILogger
{
	public Logger(@Inject("console") consoleLogger, @Inject("database") sentryLogger)
	{
	}
}
```

## How to configure

```C#
using TheOne.Extensions.DependencyInjection.Core;
using TheOne.Extensions.DependencyInjection.Loader;

// Register logging methods if needed
LoggerFactory.LogInfo = Console.WriteLine;
LoggerFactory.LogTrace = (func) =>
{
	Console.WriteLine(func());
};

// Initialize container
var services = new ServiceCollection();
var container = new AppContainer(services);

// Look for all services of matching assemblies
var loader = new AssemblyLoader
{
	Matchers = new HashSet<string>(["TheOne.Extensions.*"])
};
container.Configure(new AutoRegisterConfig(loader));

// Verify configuration
container.Verify();

```

## How to use

```C#

// Manual access

var loggers = AppContainerManager.Container.ResolveCollection<ILogger>();
var logger = AppContainerManager.Container.Resolve<ILogger>();
var logger = AppContainerManager.Container.Resolve<ILogger>("database");

// Simple case, ask for default logger

public class TestController
{
	public TestController(ILogger logger)
	{
	}
}

// Ask for all implementations
public class TestController
{
	public TestController(IEnumerable<ILogger> loggers)
	{
	}
}

// Ask for a specific implementation
public class TestController
{
	public TestController(ILogger logger)
	{
	}
}

```
