using Iridium.PluginCore;
using SampleExtensibilityCore;
using SamplePlugin;
using System.Reflection;

namespace SamplePluginHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var pluginLoader = new PluginLoader<IConsolePlugin>();
            pluginLoader.LoadPlugin(typeof(HelloWorldPlugin).GetTypeInfo().Assembly);
            foreach (var p in pluginLoader.AvailablePlugins)
            {
                p.Instance.PrintSomething();
            }
        }
    }
}