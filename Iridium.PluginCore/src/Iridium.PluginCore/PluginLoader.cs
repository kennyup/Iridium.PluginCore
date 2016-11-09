using Iridium.PluginCore.Types;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Iridium.PluginCore
{
    public class PluginLoader<T> where T : IPlugin
    {
        public List<Plugin<T>> AvailablePlugins { get; } = new List<Plugin<T>>();

        /// <summary>
        /// Loads a plugin from an assembly
        /// Once a plugin is loaded successfully, its Initialize() method is called,
        /// and the plugin is added to the AvailablePlugins collection.
        /// </summary>
        /// <param name="pluginAssembly"></param>
        public void LoadPlugin(Assembly pluginAssembly)
        {
            //Get the type that can be assigned to the target interface
            var pluginAssignableType = FindAssignableType(pluginAssembly);
            //Create a new available plugin since the type implements the IPlatinumPlugin interface
            var pluginInstance = (T)Activator.CreateInstance(pluginAssembly.GetType(pluginAssignableType.ToString()));
            var loadedPlugin = new Plugin<T>(pluginInstance, pluginAssembly);

            //Call the plugin's initialize method
            loadedPlugin.Instance.OnLoaded();

            //Add the newly loaded plugin to the plugin collection
            AvailablePlugins.Add(loadedPlugin);
        }

        /// <summary>
        /// When called, the plugin loader signals all loaded plugins to run their unload method.
        /// The list of available plugins is cleared.
        /// </summary>
        public void ShutdownPlugins()
        {
            foreach (var plugin in AvailablePlugins)
            {
                plugin.Instance.OnUnloaded();
                AvailablePlugins.Remove(plugin);
            }
        }

        public Type FindAssignableType(Assembly pluginAssembly)
        {
            Type ret = null;
            //Next we'll loop through all the Types found in the assembly
            foreach (var pluginType in pluginAssembly.GetTypes())
            {
                if (pluginType.GetTypeInfo().IsPublic)
                { //Only look at public types
                    if (!pluginType.GetTypeInfo().IsAbstract)
                    {  //Only look at non-abstract types
                       //Gets a type object of the interface we need the plugins to match
                       //Type typeInterface = pluginType.GetInterface("IPlatinumPlugin", true);
                        var containsInterface = typeof(T).GetTypeInfo().IsAssignableFrom(pluginType);
                        //Make sure the interface we want to use actually exists
                        //if (typeInterface != null)
                        if (containsInterface)
                        {
                            ret = pluginType;
                        }
                    }
                }
            }
            return ret;
        }
    }
}