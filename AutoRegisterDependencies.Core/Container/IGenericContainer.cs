using System;

namespace AutoRegisterDependencies.Core.Container
{
    /// <summary>
    /// Generic container
    /// </summary>
    public interface IGenericContainer : IContainer
    {
        /// <summary>
        /// Get the native container
        /// </summary>
        T GetNativeContainer<T>();

        /// <summary>
        /// Register the default implmentation for the given type.
        /// </summary>
        /// <param name="name"></param>
        void RegisterDefaultType<TService>(string name);

        /// <summary>
        /// Register the default implmentation for the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        void RegisterDefaultType(Type type, string name);

        /// <summary>
        /// Register default implementation for the given type
        /// </summary>
        void RegisterDefaultType<TService, TImplementation>();

        /// <summary>
        /// Register default implementation for the given type
        /// </summary>
        /// <param name="regType"></param>
        /// <param name="implType"></param>
        void RegisterDefaultType(Type regType, Type implType);

        /// <summary>
        /// Register a default instance
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="ImplT"></typeparam>
        /// <param name="instance"></param>
        void RegisterDefaultInstance<TService>(TService instance) where TService : class;

        /// <summary>
        /// Configure container (register dependencies)
        /// </summary>
        /// <param name="registerParams"></param>
        void Configure(AutoRegisterParams registerParams);

        /// <summary>
        /// Verify the container configuration
        /// </summary>
        void Verify();
    }
}
