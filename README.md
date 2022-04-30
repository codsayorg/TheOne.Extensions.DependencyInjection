# Codsay.AutoRegisterDependencies
Provide a mechanism to automatically scan and register dependencies

## Include 3 packages

### 1. Codsay.AutoRegisterDependencies.Core
Provide common attributes, core logics

### 2. Codsay.AutoRegisterDependencies.SimpleInjector
Integrate with [SimpleInjector](https://simpleinjector.org)

### 3. Codsay.AutoRegisterDependencies.DI
Integrate with [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection)

## What
Provide a ready-to-use DI container, including features:
- Automatically scan assemblies for components (repositories, services, etc)
- Support multiple (named) implementations for the same type
- Flexible configure default implementation

```C#
public interface ILogger
{
}

@Component(name = "console")
public class ConsoleLogger : ILogger
{
}

@Component(name = "sentry")
public class SentryLogger : ILogger
{
}

// Default logger when asking for ILogger
@Component()
public class Logger : ILogger
{
	public Logger(@Inject("console") consoleLogger, @Inject("sentry") sentryLogger)
	{
	}
}
```

## How
To be filled