using System;
using System.IO;
using System.Reflection;
using AliveChessPluginLibrary;
using AliveChessServer.LogicLayer;

namespace AliveChessServer
{
    public class AliveChessPluginLoader
    {
        private PluginPool _pluginPool;
        private AliveChessLogger _logger;

        private const string RelativePluginPath = @"..\Plugins";
        private const string PluginSearchPattern = @"*.dll";

        private readonly Type chatPluginType = typeof (IChatPlugin);
        private readonly Type pluginAttr = typeof (AliveChessPluginAttribute);

        public AliveChessPluginLoader(PluginPool pluginPool, AliveChessLogger logger)
        {
            _logger = logger;
            _pluginPool = pluginPool;
        }

        public void LoadPlugins(IAliveChessServerContext context)
        {
            //
        }

        public void RegisterChat(IAliveChessServerContext context)
        {
            string pluginDir = String.Concat(AppDomain.CurrentDomain.BaseDirectory, RelativePluginPath);
            string[] assemblyFiles = Directory.GetFiles(pluginDir, PluginSearchPattern);
            foreach (var assemblyFile in assemblyFiles)
            {
                Assembly assembly = Assembly.LoadFile(assemblyFile);
                Type[] types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.GetInterface(chatPluginType.Name) != null &&
                        type.IsClass && !type.IsAbstract)
                    {
                        try
                        {
                            object[] attrs = type.GetCustomAttributes(pluginAttr, false);
                            if (attrs.Length != 0 && (attrs[0] as AliveChessPluginAttribute).ForLoad)
                            {
                                object obj = Activator.CreateInstance(type);
                                IChatPlugin plugin = obj as IChatPlugin;
                                if (plugin != null)
                                {
                                    AliveChessPluginContext pluginContext =
                                        new AliveChessPluginContext(pluginDir, context);
                                    _pluginPool.LoadChat(plugin, pluginContext);
                                    _pluginPool.StartChat();
                                }
                                else
                                {
                                    _logger.Add("Plugin is null");
                                }
                            }
                        }
                        catch(Exception exception)
                        {
                            _logger.Add(string.Concat("Couldn't to load plugin: ", exception.Message));
                        }
                    }
                }
            }
        }
    }
}
