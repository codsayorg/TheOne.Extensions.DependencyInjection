using AutoRegisterDependencies.Core;
using AutoRegisterDependencies.Core.Container;
using AutoRegisterDependencies.Core.Loader;
using AutoRegisterDependencies.Core.Logger;
using SimpleInjector;
using SimpleInjector.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SI_Container = SimpleInjector.Container;

namespace AutoRegisterDependencies.SimpleInjector
{
    /// <summary>
    /// The main gate to configure injector container and its confiuration.
    /// The Register method must be called ONCE on startup time.
    /// </summary>
    public class SimpleInjectorContainer : GenericContainer
    {
        private readonly SI_Container container;

        public SimpleInjectorContainer()
        {
            container = new SI_Container();

            // Set to false. This will be the default in v5.x and going forward.
            container.Options.ResolveUnregisteredConcreteTypes = false;

            // Enable property injection by AIO.Injector.Inject attribute.
            container.Options.PropertySelectionBehavior = new PropertyBehavior();
        }

        public override T GetNativeContainer<T>()
        {
            return (T)(object)container;
        }

        public override void RegisterDefaultType(Type regType, Type implType)
        {
            container.RegisterConditional(regType, implType, Lifestyle.Singleton, c => !c.Handled);
        }

        public override void RegisterDefaultInstance<TService>(TService instance)
        {
            container.RegisterInstance(instance);
        }

        public override void Verify()
        {
            container.Verify();
        }

        public override T Resolve<T>()
        {
            return container.GetInstance<T>();
        }

        public override T Resolve<T>(Type type)
        {
            return (T)container.GetInstance(type);
        }

        public override T Resolve<T>(string name)
        {
            var collections = ResolveCollection<T>();

            var type = typeof(T);
            var registartion = container.GetCurrentRegistrations().Where(x =>
            {
                if (x.ServiceType != type) return false;

                var attr = x.ImplementationType.GetCustomAttribute<InjectableAttribute>(false);
                return attr != null && attr.Name == name;
            }).First();
            return (T)registartion.GetInstance();
        }

        public override IEnumerable<T> ResolveCollection<T>()
        {
            return container.GetAllInstances<T>();
        }

        protected override void RegisterType(Type regType, List<RegisterType> registers, bool single)
        {
            Lifestyle ls = Lifestyle.Singleton;
            switch (registers[0].Attr.Scope)
            {
                case ComponentLifetime.Transient: ls = Lifestyle.Transient; break;
                case ComponentLifetime.Scoped: ls = Lifestyle.Scoped; break;
            }

            if (registers.Count == 1)
            {
                InternalRegister(regType, registers[0], ls, true);
            }
            else
            {
                foreach (var register in registers)
                {
                    InternalRegister(regType, register, ls, false);
                }
            }

            if (!single)
            {
                container.Collection.Register(regType, registers.Select(x => x.Implementation), ls);
            }
        }

        private void InternalRegister(Type regType, RegisterType register, Lifestyle ls, bool defaultImpl)
        {
            var genericArgs = regType.GetGenericArguments();

            if (defaultImpl)
            {
                LoggerFactory.Trace(() => $"Register {regType}   -->  {register.Implementation}\r\n{register.Attr} as default and single");

                container.Register(regType, register.Implementation, ls);
            }
            else
            {
                var impAttr = register.Attr;
                var defaultType = impAttr == null || string.IsNullOrEmpty(impAttr.Name) || DefaultTypes.Any(x => x.Key == regType && x.Value == impAttr.Name);

                LoggerFactory.Trace(() => $"Register {regType} --> {register.Implementation}\r\n{register.Attr}" + (defaultType ? " as default" : null));

                container.RegisterConditional(regType, register.Implementation, ls, c =>
                {
                    // Direct GetInstance
                    if (!c.HasConsumer)
                    {
                        return defaultType;
                    }

                    // Injection
                    string name = null;
                    if (c.Consumer != null && c.Consumer.Target != null && c.Consumer.Target.Property != null)
                    {
                        var attr = c.Consumer.Target.Property.GetCustomAttribute<InjectAttribute>(inherit: true);
                        if (attr != null)
                        {
                            name = attr.Name;
                        }
                    }
                    if (!string.IsNullOrEmpty(impAttr.Name))
                    {
                        return impAttr != null && name == impAttr.Name;
                    }
                    else
                    {
                        return defaultType;
                    }
                });
            }
        }
    }

    /// <summary>
    /// Provide the ability to inject by properties.
    /// </summary>
    class PropertyBehavior : IPropertySelectionBehavior
    {
        public bool SelectProperty(Type implementationType, PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(InjectAttribute));
            return attributes != null && attributes.Any();
        }
    }

}
