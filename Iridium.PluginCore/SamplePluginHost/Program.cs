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
            //Instantiate a new plugin loader that uses plugins implement IConsolePlugin
            var pluginLoader = new PluginLoader<IConsolePlugin>();
            //Load a plugin. You can also load a plugin from a file; any instance of Assembly will work.
            pluginLoader.Factory.LoadPlugin(typeof(HelloWorldPlugin).GetTypeInfo().Assembly);
            
            
            //Runs PrintSomething() on all available plugins
            foreach (var p in pluginLoader.Factory.AvailablePlugins)
            {
                p.Instance.PrintSomething();
            }

            //Unloads all plugins
            pluginLoader.Factory.ShutdownPlugins();
        }
    }
}