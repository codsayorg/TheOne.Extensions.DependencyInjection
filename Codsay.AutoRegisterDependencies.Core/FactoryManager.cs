using Codsay.AutoRegisterDependencies.Core.Container;
using System.Collections.Generic;

namespace Codsay.AutoRegisterDependencies.Core
{
    /// <summary>
    /// The factory manager to ask for instances of types.
    /// </summary>
    public class FactoryManager
    {
        /// <summary>
        /// The global container which will be resolved when initializing application.
        /// </summary>
        public static IContainer Container { get; private set; }

        /// <summary>
        /// The detailed global container which will be resolved when initializing application.
        /// </summary>
        public static IGenericContainer DetailedContainer { get; private set; }

        /// <summary>
        /// Set a container
        /// </summary>
        /// <param name="container"></param>
        public static void SetContainer(IGenericContainer container)
        {
            Container = container;
            DetailedContainer = container;
        }

        /// <summary>
        /// Get the default implementation of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>() where T : class
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// Get a specific implementation of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Resolve<T>(string name) where T : class
        {
            return Container.Resolve<T>(name);
        }

        /// <summary>
        /// Get all implementation of the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Resolve<T>() where T : class
        {
            return Container.ResolveCollection<T>();
        }
    }
}
