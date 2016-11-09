using System;

namespace Iridium.PluginCore.Types
{
    public interface IPlugin
    {
        string Name { get; }

        string Description { get; }

        Version Version { get; }

        void OnLoaded();
    }
}