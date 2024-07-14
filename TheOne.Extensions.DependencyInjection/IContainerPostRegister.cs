namespace TheOne.Extensions.DependencyInjection;

/// <summary>
/// The loader to be performed after registering base dependencies
/// </summary>
public interface IContainerPostRegister
{
    /// <summary>
    /// Load configuration
    /// </summary>
    /// <param name="container"></param>
    void Register(IContainer container);
}