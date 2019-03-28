using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using WPF.Common.Interfaces;
using WPF.Common.Logger;

namespace DesktopDashboard.Internals
{
    internal class PluginManager
    {
        const string PluginPath = "Plugins";

        public PluginManager()
        {
            if (!Path.IsPathRooted(PluginPath))
                Directory.CreateDirectory(PluginPath);
        }

        private string GetPluginsPath()
        {
            string pluginPath = null;
            if (Path.IsPathRooted(PluginPath))
                pluginPath = PluginPath;
            else
                pluginPath = Path.Combine(Directory.GetCurrentDirectory(), PluginPath);

            return pluginPath;
        }

        public List<Plugin> GetPlugins()
        {
            List<Plugin> plugins = new List<Plugin>();
            try
            {
                string[] pluginPaths = Directory.GetFiles(GetPluginsPath(), "DI*.exe", SearchOption.AllDirectories);

                if (pluginPaths != null)
                {
                    Type pluginType = typeof(IPlugin);
                    foreach (string plugin in pluginPaths)
                    {
                        try
                        {
                            AssemblyName assemblyName = AssemblyName.GetAssemblyName(plugin);
                            if (assemblyName == null)
                                continue;
                            Assembly assembly = Assembly.Load(assemblyName);
                            if (assembly == null)
                                return null;
                            foreach (Type assemblyType in assembly.GetTypes())
                            {
                                try
                                {
                                    if (assemblyType.GetInterface(pluginType.FullName) != null)
                                    {
                                        IPlugin pluginInstance = (IPlugin)Activator.CreateInstance(assemblyType);
                                        plugins.Add(new Plugin(assemblyType, pluginInstance, plugin));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log(EventID.DesktopDashboard.Application.Exception, $"Plugin {plugin} activation", ex);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(EventID.DesktopDashboard.Application.Exception, $"Plugin {plugin} proceeding", ex);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(EventID.DesktopDashboard.Application.Exception, nameof(GetPlugins), ex);
            }
            finally
            {
                Logger.Log(EventID.DesktopDashboard.Application.FoundedPlugins, plugins.Count, String.Join(",", plugins.Select(p => p.GetPluginName())));
            }

            return plugins;
        }

    }
}
