using Iridium.PluginCore.Types;
using System.Reflection;

namespace Iridium.PluginCore
{
    public class Plugin<T> where T : IPlugin
    {
        public T Instance { get; }

        public Assembly Assembly { get; }

        public Plugin(T instance, Assembly asm)
        {
            Instance = instance;
            Assembly = asm;
        }
    }
}