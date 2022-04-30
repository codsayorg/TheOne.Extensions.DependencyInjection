using System;

namespace AutoRegisterDependencies.Core
{
    /// <summary>
    /// Request the container to provide instances of a dependency type
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
        /// <summary>
        /// Ask for the spefici implementation of type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If this dependency is optional and it is not registered a null instance will be returned,
        /// otherwise an exception will be threw
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        public InjectAttribute(string name = null)
        {
            Name = name;
        }
    }

}