using System;

namespace Iridium.PluginCore.Types
{
    public class BasePlugin : IPlugin
    {
        public virtual string Description { get; } = string.Empty;

        public virtual string Name { get; }

        public virtual Version Version { get; } = new Version(0, 0);

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