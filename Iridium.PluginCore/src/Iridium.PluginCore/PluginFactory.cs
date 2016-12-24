#region Copyright and License Header

/*

	Platinum Plugin Core

	Copyright (c) 2016 0xFireball, IridiumIon Software, ExaPhaser Industries

	Author(s): 0xFireball

	Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing
	permissions and limitations under the License.

*/

#endregion Copyright and License Header

using Iridium.PluginCore.Classes;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Iridium.PluginCore
{
    /// <summary>
    /// Stores a registry of available plugins and provides an API to access them.
    /// </summary>
    public class PluginFactory<T> where T : IPlatinumPlugin
    {
        #region Public Properties

        public PluginCollection<T> AvailablePlugins { get; }

        #endregion Public Properties

        #region Public Constructors

        public PluginFactory()
        {
            AvailablePlugins = new PluginCollection<T>();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Attempts to load a plugin from a file name. If successful, the plugin will be initialized and added the available plugin list.
        /// </summary>
        /// <param name="pluginAssembly">The assembly to attempt to load the plugin from. The parameter
        /// may be null; however, this will result in the check failing.
        /// </param>
        /// <returns>
        /// The result of the operation. If the plugin could not be loaded, the return value is false. If the plugin
        /// loads successfully, the return value will be true.
        /// </returns>
        public bool CheckPluginIsValid(Assembly pluginAssembly)
        {
            if (pluginAssembly == null) return false; //Return false if a null assembly is passed.
            return FindAssignableType(pluginAssembly) != null;
        }

        /// <summary>
        /// When called, this method clears the AvailablePlugins member of this class instance
        /// </summary>
        public void ClearPluginCollection()
        {
            AvailablePlugins.Clear();
        }

        public Type FindAssignableType(Assembly pluginAssembly)
        {
            Type ret = null;
            //Next we'll loop through all the Types found in the assembly
            foreach (var pluginType in pluginAssembly.GetTypes())
            {
                if (pluginType.IsPublic)
                { //Only look at public types
                    if (!pluginType.IsAbstract)
                    {  //Only look at non-abstract types
                       //Gets a type object of the interface we need the plugins to match
                       //Type typeInterface = pluginType.GetInterface("IPlatinumPlugin", true);
                        var containsInterface = typeof(T).IsAssignableFrom(pluginType);
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

        /// <summary>
        /// Attempts to load each plugin in the provided array of string paths.
        /// </summary>
        /// <param name="filePaths"></param>
        public void LoadPlugins(string[] filePaths)
        {
            filePaths.ToList().ForEach(fp => LoadPlugin(fp));
        }

        public void LoadPlugin(string pluginFilePath)
        {
            //Create a new assembly from the plugin file we're adding..
            var pluginAssembly = Assembly.LoadFrom(pluginFilePath);
            LoadPlugin(pluginAssembly);
        }

        /// <summary>
        /// Loads a plugin from an assembly. It is highly recommended that you check the CheckPluginIsValid()
        /// method to verify that the plugin can be loaded.
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
            var loadedPlugin = new Plugin<T>
            {
                Assembly = pluginAssembly,
                Instance = pluginInstance
            };

            //Call the plugin's initialize method
            loadedPlugin.Instance.Initialize();

            //Add the newly loaded plugin to the plugin collection
            AvailablePlugins.Add(loadedPlugin);
        }

        /// <summary>
        /// Searches the input directory for plugins
        /// </summary>
        /// <param name="directory">Directory to search for plugins</param>
        public void LoadPluginsFromDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                return; //Directory doesn't exist. We won't throw, because this might be a Plugins/ subdirectory, and this is for optionally loading plugins anyway
            //Go through all the files in the search directory
            foreach (var fileInDir in Directory.GetFiles(directory))
            {
                var file = new FileInfo(fileInDir);

                //Check if plugin can be instantiated, use an AppDomain to avoid locking up the file
                AppDomain testDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString("D"));
                var testAssembly = TryLoadAssembly(file.FullName, testDomain);
                if (CheckPluginIsValid(testAssembly))
                {
                    //Add the plugin
                    LoadPlugin(fileInDir);
                }
                AppDomain.Unload(testDomain);
            }
        }

        /// <summary>
        /// Loads plugins from the base directory of the current AppDomain.
        /// </summary>
        public void LoadPluginsInBaseDirectory()
        {
            LoadPluginsFromDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>
        /// When called, the plugin loader signals all loaded plugins to run their shutdown method simultaneously.
        /// The list of available plugins is cleared.
        /// </summary>
        public void ShutdownPlugins()
        {
            Parallel.ForEach(AvailablePlugins, availablePlugin =>
            {
                availablePlugin.Instance.Shutdown();
                availablePlugin.Instance.Dispose();
                availablePlugin.Instance = default(T); //Dereference the plugin
            });
            AvailablePlugins.Clear();
        }

        /// <summary>
        /// Attempts to load an assembly from a file path.
        /// </summary>
        /// <param name="filePath">The file path to attempt to load an assembly from.</param>
        /// <param name="targetDomain">The appdomain to load the assembly into</param>
        /// <returns>null if the operation failed, or an Assembly instance if the operation succeeded.</returns>
        public Assembly TryLoadAssembly(string filePath, AppDomain targetDomain)
        {
            try
            {
                //return Assembly.LoadFrom(filePath);
                return targetDomain.Load(File.ReadAllBytes(filePath));
            }
            catch
            {
                return null;
            }
        }

        #endregion Public Methods
    }
}