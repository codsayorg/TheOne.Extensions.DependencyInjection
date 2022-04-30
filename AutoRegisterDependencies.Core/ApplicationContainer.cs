using System;
using System.Collections.Generic;

namespace AutoRegisterDependencies.Core.Services
{
    /// <summary>
    /// A built-in container provider, to avoid calling directly to FactoryManager as a static.
    /// </summary>
    [Component]
    public class ApplicationContainer : IContainer
    {
        public T Resolve<T>() where T : class
        {
            return GetContainer().Resolve<T>();
        }

        public T Resolve<T>(Type type)
        {
            return GetContainer().Resolve<T>(type);
        }

        public T Resolve<T>(string name) where T : class
        {
            return GetContainer().Resolve<T>(name);
        }

        public IEnumerable<T> ResolveCollection<T>() where T : class
        {
            return GetContainer().ResolveCollection<T>();
        }

        private IContainer GetContainer()
        {
            return FactoryManager.Container;
        }
    }
}
