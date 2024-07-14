using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TheOne.Extensions.DependencyInjection.Loader;

internal class InjectableLoader
{
    public static void Load(AssemblyLoader assemblyLoader, Action<Type> action)
    {
        new TypeLoader()
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
                return customAttrs.Any();
            }
        }.Load(assemblyLoader, action);
    }

    

    /// <summary>
    /// Analyze the given type to get injectable base types and interfaces.
    /// </summary>
    public static IEnumerable<InjectableMetadata>? AnalyzeType(Type type)
    {
        var attrs = type.GetCustomAttributes<InjectableAttribute>().ToArray();
        if (!attrs.Any()) return null;


        var types = new List<InjectableMetadata>(attrs.Count());
        foreach (var attr in attrs)
        {
            if (attr.TargetType != null)
            {
                types.Add(new InjectableMetadata(attr.TargetType, attr));
            }
            else
            {
                var interfaces = type.GetInterfaces();
                if (interfaces.Any())
                {
                    types.Add(new InjectableMetadata(interfaces.First(), attr));
                }
            }
        }

        // TODO ! Do we really need to inject the instance itself?
        // if (!types.Any())
        // {
        //     types.Add(type);
        // }

        return types;
    }
}