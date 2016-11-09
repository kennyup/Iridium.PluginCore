using System;

namespace Iridium.PluginCore.Types
{
    public class BasePlugin : IPlugin
    {
        public event EventHandler Loaded;

        /// <summary>
        /// Called when the plugin is loaded. The default implementation invokes the Loaded event.
        /// </summary>
        public virtual void OnLoaded()
        {
            Loaded?.Invoke(this, EventArgs.Empty);
        }
    }
}