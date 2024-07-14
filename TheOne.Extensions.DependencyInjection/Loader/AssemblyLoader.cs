using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TheOne.Extensions.DependencyInjection.Loader;

public class AssemblyLoader
{
    /// <summary>
    /// Search pattern to load files.
    /// Default: *.dll
    /// </summary>
    public string SearchPattern { get; set; } = "*.dll";
    
    /// <summary>
    /// Regexes to match assembly files
    /// </summary>
    public ISet<string>? Matchers { get; init; }
    
    public void Load(Action<Assembly> actionOnAssembly)
    {
        IEnumerable<string> assemblyFiles = Directory
            .GetFiles(AppDomain.CurrentDomain.BaseDirectory, SearchPattern);

        if (Matchers != null && Matchers.Any())
        {
            assemblyFiles = assemblyFiles.Where(x => new Regex(x, RegexOptions.IgnoreCase).IsMatch(x));
        }
        
        foreach (var file in assemblyFiles)
        {
            var assembly = Assembly.Load(AssemblyName.GetAssemblyName(file));
            actionOnAssembly(assembly);
        }
    }
}
