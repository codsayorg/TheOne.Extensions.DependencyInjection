using Microsoft.Extensions.DependencyInjection;
using TheOne.Extensions.DependencyInjection.Core;
using TheOne.Extensions.DependencyInjection.Loader;
using TheOne.Extensions.DependencyInjection.Microsoft.Tests.Artifacts;

namespace TheOne.Extensions.DependencyInjection.Microsoft.Tests;

public class SingletonServiceTests
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
        var services = AppContainerManager.Container.ResolveCollection<IKeyedSingletonService>();
        Assert.That(services.Count(), Is.EqualTo(3));
    }
    
    [Test]
    public void Singleton_Keyed_Single()
    {
        var service = AppContainerManager.Container.Resolve<IKeyedSingletonService>();
        Assert.That(service, Is.Not.Null);
        Assert.That(typeof(KeyedSingletonService1), Is.EqualTo(service.GetType()));
    }
    
    [Test]
    public void Singleton_Keyed_ByKey()
    {
        var service = AppContainerManager.Container.Resolve<IKeyedSingletonService>("KeyedSingletonService2");
        Assert.That(typeof(KeyedSingletonService2), Is.EqualTo(service.GetType()));
    }
}