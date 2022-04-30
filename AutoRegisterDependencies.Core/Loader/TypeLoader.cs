using System;

namespace AutoRegisterDependencies.Core.Loader
{
    /// <summary>
    /// Parameters to load
    /// </summary>
    public class TypeLoaderParams
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
    }

    /// <summary>
    /// Type loader
    /// </summary>
    public class TypeLoader
    {
        /// <summary>
        /// Load types according to the given filters and perform action on it.
        /// </summary>
        /// <param name="paras"></param>
        public void Load(AssemblyLoaderParams assemblyLoaderParams, TypeLoaderParams typeLoaderParams, Action<Type> action)
        {
            AssemblyLoader.Load(assemblyLoaderParams, (assembly) =>
            {
                var types = assembly.GetTypes();
                foreach (var t in types)
                {
                    if (typeLoaderParams.ClassesOnly && !t.IsClass)
                    {
                        continue;
                    }

                    if (!typeLoaderParams.FilterFunc(t))
                    {
                        continue;
                    }

                    action(t);
                }
            });
        }
    }
}
