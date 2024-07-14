using System;

namespace TheOne.Extensions.DependencyInjection;

/// <summary>
/// Lifetime of instances
/// </summary>
public enum ComponentLifetime
{
    /// <summary>
    /// Singleton - Application wide
    /// </summary>
    Singleton,

    /// <summary>
    /// For applications that process requests (web apps), scoped lifetime indicates that components are created once per request.
    /// Instances of components are disposed at the end of request.
    /// </summary>
    Scoped,

    /// <summary>
    /// Create a new instance each time they're requested from the container
    /// </summary>
    Transient
}

/// <summary>
/// The attribute to register all injectable components (class level)
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class InjectableAttribute() : Attribute
{
    /// <summary>
    /// The life-time of instance.
    /// </summary>
    public ComponentLifetime Scope { get; set; } = ComponentLifetime.Singleton;

    /// <summary>
    /// Register a specific name to the current registration.
    /// </summary>
    /// <example>
    /// We have the Logger class, we want to have the default one (log to DB), log to file, log to logging service XXX.
    /// We would have 3 different implementation classes.
    /// class DBLogger : Logger => @Injectable
    /// class FileLogger : Logger => @Injectable(Name = "File")
    /// class XXXLogger : Logger => @Injectable(Name = "XXX")
    /// 
    /// And in the places we want to use:
    /// @Inject() Logger => Returns the default instance.
    /// @Inject(Name = "File") => Returns the FileLogger
    /// @Inject(Name = "XXX") => Returns the XXXLogger.
    /// </example>
    public string? Name { get; set; }
    
    /// <summary>
    /// Register a type to be used as key to retrieve instances.
    /// Leave null to use the first interface as key
    /// </summary>
    public Type? TargetType { get; set; }

    /// <summary>
    /// The priority to register (higher will be registered later and highest priority will be default)
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <inheritdoc/>
    public override string ToString()
    {
        return !string.IsNullOrEmpty(Name) ? $"with name {Name} and scope {Scope}" : $"without name and scope {Scope}";
    }
}

/// <summary>
/// The attribute to load container registration
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ContainerRegisterAttribute : InjectableAttribute
{
}

/// <summary>
/// Singleton (Application wide) scope components
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ComponentAttribute : InjectableAttribute
{
}

/// <summary>
/// Singleton (Application wide) scope services
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceAttribute : ComponentAttribute
{
}

/// <summary>
/// Singleton (Application wide) scope repositories
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RepositoryAttribute : ComponentAttribute
{
}

/// <summary>
/// When we have multiple implementation for a type,
/// we should provide this attribute to the one which we expect it to be provided as default (if name is not provided)
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DefaultImplementationAttribute() : Attribute
{
}