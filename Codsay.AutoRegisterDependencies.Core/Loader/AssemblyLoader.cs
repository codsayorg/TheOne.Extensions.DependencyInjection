using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Codsay.AutoRegisterDependencies.Core.Loader
{
    /// <summary>
    /// Parameters to load
    /// </summary>
    public class AssemblyLoaderParams
    {
        /// <summary>
        /// Load specific assemblies which have namespace starts with expected names
        /// </summary>
        public ISet<string> Namespaces { get; set; }
    }

    /// <summary>
    /// Load assemblies with filters
    /// </summary>
    public class AssemblyLoader
    {
        /// <summary>
        /// Load assemblies according to the given filters and perform action on each.
        /// </summary>
        /// <param name="paras"></param>
        public static void Load(AssemblyLoaderParams paras, Action<Assembly> action)
        {
            var asls = Directory
                .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));

            if (paras.Namespaces != null && paras.Namespaces.Any())
            {
                var namespaces = paras.Namespaces.Select(ns => ns + ".");
                asls = asls.Where(x => namespaces.Any(ns =>
                    x.GetName().Name.StartsWith(ns, StringComparison.OrdinalIgnoreCase)
                    || ns.Equals(x.GetName().Name + ".", StringComparison.OrdinalIgnoreCase))
                ).ToArray();
            }

            foreach (var asl in asls)
            {
                action(asl);
            }
        }
    }
}
