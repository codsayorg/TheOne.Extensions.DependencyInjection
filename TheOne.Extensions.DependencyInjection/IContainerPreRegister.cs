namespace TheOne.Extensions.DependencyInjection;

/// <summary>
/// The loader to be performed before registering base dependencies
/// </summary>
public interface IContainerPreRegister
{
    /// <summary>
    /// Load configuration
    /// </summary>
    /// <param name="container"></param>
    void Register(IContainer container);
}