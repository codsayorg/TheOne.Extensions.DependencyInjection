namespace AutoRegisterDependencies.Core.Container
{
    /// <summary>
    /// The loader to be peformed after registering base dependencies
    /// </summary>
    public interface IContainerPostRegister
    {
        /// <summary>
        /// Load configuration
        /// </summary>
        /// <param name="container"></param>
        void Register(IGenericContainer container);
    }
}
