using System.Diagnostics.CodeAnalysis;

namespace TheOne.Extensions.DependencyInjection;

public interface IGenericContainer<out TContainer> : IContainer
{
    [return: NotNull]
    TContainer GetNativeContainer();

    void Configure(AutoRegisterConfig registerConfig);

    /// <summary>
    /// Verify the container configuration
    /// </summary>
    void Verify();
}