using System;

namespace TheOne.Extensions.DependencyInjection.Loader;

/// <summary>
/// Parameters to load
/// </summary>
public class TypeLoader
{
    /// <summary>
    /// Filter only classes.
    /// Default value is true.
    /// </summary>
    public bool ClassesOnly { get; set; } = true;

    /// <summary>
    /// Advantage and flexible filter function to apply on types.
    /// </summary>
    public Func<Type, bool> FilterFunc { get; set; } = (t) => true;
    
    /// <summary>
    /// Load types according to the given filters and perform action on it.
    /// </summary>
    public void Load(AssemblyLoader cfg, Action<Type> action)
    {
        cfg.Load((assembly) =>
        {
            var types = assembly.GetTypes();
            foreach (var t in types)
            {
                if (ClassesOnly && !t.IsClass)
                {
                    continue;
                }

                if (!FilterFunc(t))
                {
                    continue;
                }

                action(t);
            }
        });
    }
}

