using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoRegisterDependencies.Core.Loader
{
    public class InjectableLoader
    {
        public void Load(AssemblyLoaderParams assemblyLoaderParams, Action<Type> action)
        {
            new TypeLoader().Load(assemblyLoaderParams, new TypeLoaderParams()
            {
                FilterFunc = (t) =>
                {
                    // Only accepts real implementation
                    if (t.IsAbstract || t.IsInterface)
                    {
                        return false;
                    }
 
                    // Only accepts implementations which have defined injectable attribute
                    var customAttrs = t.GetCustomAttributes(typeof(InjectableAttribute), false);
                    if (customAttrs != null && customAttrs.Any())
                    {
                        return true;
                    }

                    return false;
                }
            }, action);
        }

        /// <summary>
        /// Analyze the given type to get injectable base types and interfaces.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attr"></param>
        public static List<Type> AnalyzeType(Type type, out InjectableAttribute injectAttr)
        {
            injectAttr                   = type.GetCustomAttribute<InjectableAttribute>(false);
            var typesWithinjectAttribute = new List<Type>();

            if (injectAttr.Including.HasFlag(IncludingType.BaseClasses))
            {
                var baseType = type.BaseType;
                while (baseType != null)
                {
                    typesWithinjectAttribute.Add(baseType);
                    baseType = baseType.BaseType;
                }
            }

            if (injectAttr.Including.HasFlag(IncludingType.Interfaces))
            {
                var baseItfs = type.GetInterfaces();
                if (baseItfs != null)
                {
                    foreach (var itf in baseItfs)
                    {
                        typesWithinjectAttribute.Add(itf);
                    }
                }
            }

            if ((typesWithinjectAttribute == null || !typesWithinjectAttribute.Any()) || injectAttr.Including.HasFlag(IncludingType.Implementation))
            {
                typesWithinjectAttribute.Add(type);
            }

            return typesWithinjectAttribute;
        }
    }
}
