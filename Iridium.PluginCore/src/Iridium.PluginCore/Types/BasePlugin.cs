using System;

namespace Iridium.PluginCore.Types
{
    public class BasePlugin : IPlugin
    {
        public event EventHandler Loaded;

        public void OnLoaded()
        {
            Loaded?.Invoke(this, EventArgs.Empty);
        }
    }
}