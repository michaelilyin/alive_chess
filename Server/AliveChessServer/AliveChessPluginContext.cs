using AliveChessPluginLibrary;

namespace AliveChessServer
{
    public class AliveChessPluginContext : IAliveChessPluginContext
    {
        private string _pluginDir;
        private IAliveChessServerContext _server;

        public AliveChessPluginContext(string pluginDir, IAliveChessServerContext server)
        {
            this._pluginDir = pluginDir;
            this._server = server;
        }

        public string PluginDir
        {
            get { return _pluginDir; }
        }

        public IAliveChessServerContext Server
        {
            get { return _server; }
        }
    }
}
