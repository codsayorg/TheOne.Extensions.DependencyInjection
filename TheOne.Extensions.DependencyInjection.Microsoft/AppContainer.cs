using Microsoft.Extensions.DependencyInjection;
using TheOne.Extensions.DependencyInjection.Core;
using TheOne.Extensions.DependencyInjection.Loader;
#pragma warning disable CS8604 // Possible null reference argument.

namespace TheOne.Extensions.DependencyInjection.Microsoft;

public class AppContainer : GenericContainer<IServiceCollection>
{
    // // Set to false. This will be the default in v5.x and going forward.
    // container.Options.ResolveUnregisteredConcreteTypes = false;
    //
    // // Enable property injection by AIO.Injector.Inject attribute.
    // container.Options.PropertySelectionBehavior = new PropertyBehavior();

    private readonly IServiceCollection _container;
    private IServiceProvider? _provider;
    
    public AppContainer(IServiceCollection container)
    {
        _container = container;
    }

    public override IServiceCollection GetNativeContainer()
    {
        return _container;
    }

    public override void Verify()
    {
        _provider = _container.BuildServiceProvider();
    }

    public override T Resolve<T>(string? name = null)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        return (string.IsNullOrEmpty(name) ? _provider.GetService<T>() : _provider.GetKeyedService<T>(name))!;
#pragma warning restore CS8604 // Possible null reference argument.
    }

    public override IEnumerable<T> ResolveCollection<T>()
    {
#pragma warning disable CS8604 // Possible null reference argument.
        return _provider.GetServices<T>();
#pragma warning restore CS8604 // Possible null reference argument.
    }

    protected override void RegisterType(IEnumerable<RegisterType> registers)
    {
        foreach (var ((type, attr), implType) in registers)
        {
            LoggerFactory.Trace(() => $"Register for {implType}");

            var serviceKey = attr.Name;
            if (string.IsNullOrEmpty(serviceKey))
            {
                LoggerFactory.Trace(() => $"-- {type}");
                
                switch (attr.Scope)
                {
                    case ComponentLifetime.Singleton:
                        _container.AddSingleton(type, implType);
                        break;
                    case ComponentLifetime.Scoped:
                        _container.AddScoped(type, implType);
                        break;
                    case ComponentLifetime.Transient:
                        _container.AddTransient(type, implType);
                        break;
                }
            }
            else
            {
                LoggerFactory.Trace(() => $"-- {type}: as key {serviceKey}");
                
                switch (attr.Scope)
                {
                    case ComponentLifetime.Singleton:
                        _container.AddSingleton(type, implType);
                        _container.AddKeyedSingleton(type, serviceKey, implType);
                        break;
                    case ComponentLifetime.Scoped:
                        _container.AddScoped(type, implType);
                        _container.AddKeyedScoped(type, serviceKey, implType);
                        break;
                    case ComponentLifetime.Transient:
                        _container.AddTransient(type, implType);
                        _container.AddKeyedTransient(type, serviceKey, implType);
                        break;
                }
            }
        }
    }
}