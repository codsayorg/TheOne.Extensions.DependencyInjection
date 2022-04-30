namespace AutoRegisterDependencies.Core.Container
{
    /// <summary>
    /// The loader to be peformed before registering base dependencies
    /// </summary>
    public interface IContainerPreRegister
    {
        /// <summary>
        /// Load configuration
        /// </summary>
        /// <param name="container"></param>
        void Register(IGenericContainer container);
    }
}
