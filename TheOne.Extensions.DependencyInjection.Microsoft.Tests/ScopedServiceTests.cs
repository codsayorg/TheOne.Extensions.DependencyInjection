using Microsoft.Extensions.DependencyInjection;
using TheOne.Extensions.DependencyInjection.Core;
using TheOne.Extensions.DependencyInjection.Loader;
using TheOne.Extensions.DependencyInjection.Microsoft.Tests.Artifacts;

namespace TheOne.Extensions.DependencyInjection.Microsoft.Tests;

public class ScopedServiceTests
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
    public void Singleton_AsExpected()
    {
        var service = AppContainerManager.Container.Resolve<ISingletonService>();
        Assert.That(service, Is.Not.Null);
    }
    
    [Test]
    public void Singleton_Keyed_All()
    {
        var services = AppContainerManager.Container.ResolveCollection<IKeyedScopedService>();
        Assert.That(services.Count(), Is.EqualTo(3));
    }
    
    [Test]
    public void Singleton_Keyed_Single()
    {
        var service = AppContainerManager.Container.Resolve<IKeyedScopedService>();
        Assert.That(typeof(KeyedScopedService1), Is.EqualTo(service.GetType()));
    }
}