using System;
using System.Collections.Generic;
using System.Threading;
using AliveChessPluginLibrary;

namespace AliveChessServer
{
    public class PluginPool
    {
        private IChatPlugin _chatPlugin;
        private Thread _chatPluginThread;
        private List<IAliveChessPlugin> _plugins;

        public PluginPool()
        {
            _plugins = new List<IAliveChessPlugin>();
        }

        public void LoadPlugin(IAliveChessPlugin plugin, 
            IAliveChessPluginContext context)
        {
            _plugins.Add(plugin);
            plugin.AfterCreate(context);
        }

        public void LoadChat(IChatPlugin plugin, 
            IAliveChessPluginContext context)
        {
            if (!IsChatLoaded)
            {
                plugin.AfterCreate(context);
                _chatPlugin = plugin;
            }
        }

        public void UnloadChat()
        {
            if (IsChatLoaded)
            {
                _chatPlugin.BeforeDestroy();
                _chatPlugin = null;
            }
        }

        public void UnloadPlugin(IAliveChessPlugin plugin)
        {
            plugin.BeforeDestroy();
            _plugins.Remove(plugin);
        }

        public bool IsChatLoaded
        {
            get { return _chatPlugin != null; }
        }

        public void StartChat()
        {
            if (IsChatLoaded)
            {
                _chatPluginThread = new Thread(Execute)
                              {
                                  IsBackground = true
                              };

                _chatPluginThread.Start();
            }
        }

        private void Execute()
        {
            while (true)
            {
                try
                {
                    _chatPlugin.Execute();
                }
                catch (Exception ex)
                {
                    _chatPlugin.Receive(ex);
                }
                
                Thread.Sleep(1000);
            }
        }

        public void StopChat()
        {
            _chatPluginThread.Abort();
        }

        public IChatPlugin Chat
        {
            get { return _chatPlugin; }
        }
    }
}
