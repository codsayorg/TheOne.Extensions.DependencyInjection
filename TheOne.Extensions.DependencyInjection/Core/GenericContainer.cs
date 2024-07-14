using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TheOne.Extensions.DependencyInjection.Loader;

namespace TheOne.Extensions.DependencyInjection.Core;

public abstract class GenericContainer<TContainer> : IGenericContainer<TContainer>
{
    public virtual void Configure(AutoRegisterConfig registerConfig)
    {
        LoggerFactory.Info("Start configuring container");

        // Type (direct type, base types, interfaces)
        var injectableKeyTypes = new Dictionary<Type, List<RegisterType>>();
        var containerPreRegisters = new List<IContainerPreRegister>();
        var containerPostRegisters = new List<IContainerPostRegister>();

        LoggerFactory.Info("Step 1: Load all potential types");
        InjectableLoader.Load(registerConfig.Loader, implT =>
        {
            var injectableInfos = InjectableLoader.AnalyzeType(implT);
            if (injectableInfos == null || !injectableInfos.Any())
            {
                return;
            }

            foreach (var injectableType in injectableInfos)
            {
                var attr = injectableType.Attr;
                if (attr is ContainerRegisterAttribute)
                {
                    LoggerFactory.Trace(() => $"Analyzed type {implT} - which is a container register");

                    var register = Activator.CreateInstance(implT);
                    switch (register)
                    {
                        case IContainerPreRegister preRegister:
                            containerPreRegisters.Add(preRegister);
                            break;
                        case IContainerPostRegister postRegister:
                            containerPostRegisters.Add(postRegister);
                            break;
                        default:
                            throw new AutoRegisterException($"The register must implement either {nameof(IContainerPreRegister)} or {nameof(IContainerPostRegister)}");
                    }
                }
                else
                {
                    LoggerFactory.Trace(() => $"Analyzed type {implT} - has {injectableInfos.Count()} register(s)");

                    foreach (var item in injectableInfos)
                    {
                        if (!injectableKeyTypes.TryGetValue(item.Type, out var types))
                        {
                            types = [];
                            injectableKeyTypes[item.Type] = types;
                        }

                        types.Add(new RegisterType(item, implT));
                    }
                }
            }
        });

        LoggerFactory.Info("Step 2: Pre-register container");
        foreach (var item in containerPreRegisters) item.Register(this);

        LoggerFactory.Info("Step 3: Register types");
        foreach (var item in injectableKeyTypes)
        {
            RegisterType(item.Value.OrderBy(x => x.Info.Attr.Priority));
        }

        LoggerFactory.Info( "Step 4: Post-register container");
        foreach (var item in containerPostRegisters) item.Register(this);
        
        LoggerFactory.Info($"Set the initialized container {GetType()} to container manager");
        AppContainerManager.SetContainer(this);
    }

    [return: NotNull]
    public abstract TContainer GetNativeContainer();
    
    public abstract void Verify();
    
    public abstract T Resolve<T>(string? name = null);
    
    public abstract IEnumerable<T> ResolveCollection<T>();
    
    protected abstract void RegisterType(IEnumerable<RegisterType> registers);
}