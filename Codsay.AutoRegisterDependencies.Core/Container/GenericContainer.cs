using Codsay.AutoRegisterDependencies.Core.Loader;
using Codsay.AutoRegisterDependencies.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codsay.AutoRegisterDependencies.Core.Container
{
    /// <summary>
    /// Default implementation for a container
    /// </summary>
    public abstract class GenericContainer : IGenericContainer
    {
        protected Dictionary<Type, string> DefaultTypes { get; } = new Dictionary<Type, string>();

        public abstract T GetNativeContainer<T>();

        public void RegisterDefaultType(Type type, string name)
        {
            DefaultTypes[type] = name;
        }

        public void RegisterDefaultType<T>(string name)
        {
            RegisterDefaultType(typeof(T), name);
        }

        public void RegisterDefaultType<TService, TImplementation>()
        {
            RegisterDefaultType(typeof(TService), typeof(TImplementation));
        }

        public abstract void RegisterDefaultType(Type regType, Type implType);

        public abstract void RegisterDefaultInstance<TService>(TService instance) where TService : class;

        protected abstract void RegisterType(Type regType, List<RegisterType> registers, bool single);

        public virtual void Configure(AutoRegisterParams registerParams)
        {
            LoggerFactory.Info("Start configuring container");

            // Type (direct type, base types, interfaces)
            var injectableTypes = new Dictionary<Type, List<RegisterType>>();
            var containerPreRegisters = new List<IContainerPreRegister>();
            List<IContainerPostRegister> containerPostRegisters = new List<IContainerPostRegister>();

            new InjectableLoader().Load(registerParams.AssemblyParams, implT =>
            {
                var typesToRegister = InjectableLoader.AnalyzeType(implT, out var attr);
                if (attr is ContainerRegisterAttribute)
                {
                    LoggerFactory.Trace(() => $"Analyzed type {implT} - which is a container register");

                    var register = Activator.CreateInstance(implT);
                    if (register is IContainerPreRegister preRegister)
                    {
                        containerPreRegisters.Add(preRegister);
                    }
                    else if (register is IContainerPostRegister postRegister)
                    {
                        containerPostRegisters.Add(postRegister);
                    }
                    else
                    {
                        throw new AutoRegisterException($"The register must implement either {nameof(IContainerPreRegister)} or {nameof(IContainerPostRegister)}");
                    }
                }
                else
                {
                    LoggerFactory.Trace(() => $"Analyzed type {implT} - has {typesToRegister.Count} register(s)");

                    foreach (var item in typesToRegister)
                    {
                        if (!injectableTypes.ContainsKey(item))
                        {
                            injectableTypes[item] = new List<RegisterType>();
                        }

                        injectableTypes[item].Add(new RegisterType(attr, implT));
                    }
                }
            });

            LoggerFactory.Trace(() => "Start register types");
            LoggerFactory.Trace(() => $"Default types:\r\n{string.Join("\r\n", DefaultTypes.Select(x => x.Key + ": " + x.Value).OrderBy(x => x))}");

            foreach (var item in containerPreRegisters) item.Register(this);

            foreach (var item in injectableTypes)
            {
                RegisterType(item.Key, item.Value, injectableTypes.Count <= 1);
            }

            foreach (var item in containerPostRegisters) item.Register(this);
        }

        public abstract void Verify();

        public abstract T Resolve<T>() where T : class;

        public abstract T Resolve<T>(Type type);

        public abstract T Resolve<T>(string name) where T : class;

        public abstract IEnumerable<T> ResolveCollection<T>() where T : class;

    }
}
