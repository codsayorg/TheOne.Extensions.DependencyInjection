using System;
using System.Collections.Generic;

namespace AutoRegisterDependencies.Core
{
    /// <summary>
    /// Container
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Reolve the given type to an instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Reolve the given type to an instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>(Type type);

        /// <summary>
        /// Resolve the given named type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T Resolve<T>(string name) where T : class;

        /// <summary>
        /// Get all instance register to the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveCollection<T>() where T : class;
    }
}
