using System;

namespace Codsay.AutoRegisterDependencies.Core
{
    /// <summary>
    /// Lifetime of instances
    /// </summary>
    public enum ComponentLifetime
    {
        /// <summary>
        /// Singleton - Application wide
        /// </summary>
        Singleton,

        /// <summary>
        /// For applications that process requests (web apps), scoped lifetime indicates that components are created once per request.
        /// Instances of components are disposed at the end of request.
        /// </summary>
        Scoped,

        /// <summary>
        /// Create a new instance each time they're requested from the container
        /// </summary>
        Transient
    }

    [Flags]
    public enum IncludingType
    {
        /// <summary>
        /// Interfaces only
        /// </summary>
        Interfaces = 0x0001,

        /// <summary>
        /// Base classes only
        /// </summary>
        BaseClasses = 0x0010,

        /// <summary>
        /// Include the implementation where defines the injection
        /// </summary>
        Implementation = 0x0100,

        /// <summary>
        /// Everything (Interfaces and base classes)
        /// </summary>
        Everything = Interfaces | BaseClasses | Implementation
    }

    /// <summary>
    /// The attribute to register all injectable components (class level)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InjectableAttribute : Attribute
    {
        /// <summary>
        /// The lifetime of instance.
        /// </summary>
        public virtual ComponentLifetime Scope { get; set; }

        /// <summary>
        /// Register a specific name to the current registration.
        /// </summary>
        /// <example>
        /// We have the Logger class and we want to have the default one (log to DB), log to file, log to logging service XXX.
        /// We would have 3 different implementation classes.
        /// class DBLogger : Logger => @Injectable
        /// class FileLogger : Logger => @Injectable(Name = "File")
        /// class XXXLogger : Logger => @Injectable(Name = "XXX")
        /// 
        /// And in the places we want to use:
        /// @Inject() Logger => Returns the default instance.
        /// @Inject(Name = "File") => Returns the FileLogger
        /// @Inject(Name = "XXX") => Returns the XXXLogger.
        /// </example>
        public string Name { get; set; }

        /// <summary>
        /// Including types to register as key (type=implementaionType) in the container
        /// </summary>
        public IncludingType Including { get; set; } = IncludingType.Interfaces;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        public InjectableAttribute(string name = null, ComponentLifetime scope = ComponentLifetime.Singleton)
        {
            Name = name;
            Scope = scope;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Name != null && Name.Length > 0)
            {
                return $"with name {Name} and scope {Scope}";
            }
            else
            {
                return $"without name and scope {Scope}";
            }
        }
    }

    /// <summary>
    /// Singleton (Application wide) scope components
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ComponentAttribute : InjectableAttribute
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        public ComponentAttribute(string name = null, ComponentLifetime scope = ComponentLifetime.Singleton) : base(name, scope)
        {
        }
    }

    /// <summary>
    /// Singleton (Application wide) scope data-context
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DataContextAttribute : ComponentAttribute
    {
    }

    /// <summary>
    /// Singleton (Application wide) scope services
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceAttribute : ComponentAttribute
    {
    }

    /// <summary>
    /// Singleton (Application wide) scope repositories
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RepositoryAttribute : ComponentAttribute
    {
    }

    /// <summary>
    /// Request scope dependencies
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequestAttribute : ComponentAttribute
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        public RequestAttribute(string name = null, ComponentLifetime scope = ComponentLifetime.Scoped) : base(name, scope)
        {
        }
    }

    /// <summary>
    /// Transient scope dependencies
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TransientAttribute : ComponentAttribute
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        public TransientAttribute(string name = null, ComponentLifetime scope = ComponentLifetime.Transient) : base(name, scope)
        {
        }
    }

    /// <summary>
    /// When we have multiple implementation for a type,
    /// we should provide this attribute to the one which we expect it to be be provided as default (if name is not provided)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class DefaultImplementationAttribute : Attribute
    {
        /// <summary>
        /// Implementation type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        public DefaultImplementationAttribute(Type type = null)
        {
            Type = type;
        }
    }
}
