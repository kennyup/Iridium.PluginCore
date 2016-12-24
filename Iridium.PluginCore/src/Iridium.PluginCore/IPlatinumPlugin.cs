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

using Platinum.PluginCore3.Classes;
using System;

namespace Platinum.PluginCore3
{
    public interface IPlatinumPlugin
    {
        #region Public Properties

        /// <summary>
        /// A string representing the name or names of the authors of the plugin
        /// </summary>
        string Author { get; }

        /// <summary>
        /// A description of the plugin as it will appear to the program loading it.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// A global unique identifier. It is recommended that this be a constant string in the form of a GUID.
        /// </summary>
        string PluginGuid { get; }

        /// <summary>
        /// A property representing the plugin host
        /// </summary>
        IPlatinumPluginHost Host { get; set; }

        /// <summary>
        /// The name of the plugin as it will be visible to the program that is loading it
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A string that represents the key name of settings that can be specified in the configuration file. This is processed by the program loading the plugins
        /// </summary>
        string PreferencesKey { get; }

        /// <summary>
        /// A version string representing the version number of the plugin. It is highly recommended that you use semver (semantic versioning)
        /// </summary>
        Version Version { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// A method that is called to configure the plugin with additional settins. Note that this will only be called if additional settings are explicitly specified. The program loading the plugin has control over this.
        /// </summary>
        /// <param name="pluginSettings"></param>
        void ConfigurePlugin(PluginPreferences pluginSettings);

        /// <summary>
        /// This method is automatically called by the loader when shutting down plugins. This is called after Shutdown() is called
        /// </summary>
        void Dispose();

        /// <summary>
        /// This method is automatically called by the loader when the plugin is loaded
        /// </summary>
        void Initialize();

        /// <summary>
        /// This method is called by the program loading the plugins.
        /// It is not guaranteed to be called (though it is recommended) but it allows the plugin to prepare itself independently
        /// of the Initialize() call.
        /// This method is intended to be used to set up the various components of the plugin.
        /// </summary>
        void LoadComponents();

        /// <summary>
        /// This method is called by the program loading the plugins.
        /// It is not guaranteed to be called.
        /// This method is intended to be used as a counterpart to LoadComponents().
        /// You can use this to suspend your plugin without actually shutting it down or disposing it.
        /// </summary>
        void UnloadComponents();

        /// <summary>
        /// This method is called by the loader automatically. This allows the plugin to shut down its components.
        /// </summary>
        void Shutdown();

        #endregion Public Methods
    }
}