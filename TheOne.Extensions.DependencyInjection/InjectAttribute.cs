using System;

namespace TheOne.Extensions.DependencyInjection;

/// <summary>
/// Request the container to provide instances of a dependency type
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class InjectAttribute(string? name = null) : Attribute
{
    /// <summary>
    /// Ask for the specific implementation of type.
    /// </summary>
    public string? Name { get; set; } = name;

    /// <summary>
    /// If this dependency is optional, and it is not registered a null instance will be returned,
    /// otherwise an exception will be thrown
    /// </summary>
    public bool Optional { get; set; }
}