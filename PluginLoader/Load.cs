using PluginContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plugins
{
    class PluginLoader
    {
        readonly static string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        private static ICollection<IPlugin> Plugins;
        public static ICollection<IPlugin> Load(string[] ToLoad)
        {
            List<string> dllFileNames = new List<string>();

            if (Directory.Exists(path))
            {
                // Search if the assemblies of the configuration file exists
                foreach (string Plugin in ToLoad)
                {
                    string AccessPath = Path.Combine(path, Plugin);
                    if (File.Exists(AccessPath))
                        dllFileNames.Add(AccessPath);
                    else
                        Console.WriteLine("The module '{0}' not exists", Plugin);
                }
                // Next we have to load the assemblies
                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Count);
                foreach (string dllFile in dllFileNames)
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(an);
                    assemblies.Add(assembly);
                }
                // We can search for all types that implement our Interface IPlugin.
                Type pluginType = typeof(IPlugin);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();
                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }
                            else
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    pluginTypes.Add(type);
                                }
                            }
                        }
                    }
                }
                // Last we create instances from our found types using Reflections.
                Plugins = new List<IPlugin>(pluginTypes.Count);
                foreach (Type type in pluginTypes)
                {
                    IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                    Plugins.Add(plugin);
                }
            }
            return Plugins;
        }
            
    }
}
