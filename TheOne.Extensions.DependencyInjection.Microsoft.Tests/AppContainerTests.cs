using Microsoft.Extensions.DependencyInjection;
using TheOne.Extensions.DependencyInjection.Core;
using TheOne.Extensions.DependencyInjection.Loader;

namespace TheOne.Extensions.DependencyInjection.Microsoft.Tests;

public class AppContainerTests
{
    [SetUp]
    public void Setup()
    {
        LoggerFactory.LogInfo = Console.WriteLine;
        LoggerFactory.LogTrace = (func) =>
        {
            Console.WriteLine(func());
        };
        
        var services = new ServiceCollection();
        var container = new AppContainer(services);
        container.Configure(new AutoRegisterConfig(new AssemblyLoader
        {
            Matchers = new HashSet<string>(["TheOne.Extensions.*"])
        }));
        container.Verify();
    }

    [Test]
    public void AppContainer_NotNull()
    {
        Assert.That(AppContainerManager.Container, Is.Not.Null);
    }
    
    [Test]
    public void AppContainer_NativeContainer_NotNull()
    {
        var container = AppContainerManager.Container as AppContainer;
        Assert.That(container, Is.Not.Null);

        var nativeContainer = container.GetNativeContainer();
        Assert.That(nativeContainer, Is.Not.Null);
    }
}